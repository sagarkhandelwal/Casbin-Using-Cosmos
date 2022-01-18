using CasbinRBAC.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCasbin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceTokenConsole;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CasbinRBAC.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CasbinController : Controller
    {
        #region Fields
        private readonly IConfiguration _configuration;
        private readonly Enforcer _enforcer;
        private readonly string unauthorised = "unauthorised user";
        #endregion

        #region Ctor
        public CasbinController(CasbinDbContext<int> databaseContext, IConfiguration configuration)
        {
            _configuration = configuration;
            var efAdapter = new CasbinDbAdapter<int>(databaseContext);
            _enforcer = new Enforcer("CasbinConfig/rbac_model.conf", efAdapter);
        }
        #endregion


        #region DBValues
        public static readonly string endpoint = "https://pocdb1.documents.azure.com:443/";
        public static readonly string dbName = "UserManagementDemoDb";
        public static readonly string accountKey = "zgoR8kf3hQRrzdA2dFtV2n6gotxwsLADdXSQfRvyBbgc3P2wsPeZ11Xl6NvPk5581UwGmaQl6A3XjQDXxLSJPg==";
        #endregion

        /// <summary>
        /// sub is for user
        /// dom is for aff
        /// obj is for roleName
        /// act is for privilege
        /// </summary>
        /// <param name="user"></param>
        /// <param name="aff"></param>
        /// <param name="roleName"></param>
        /// <param name="privilege"></param>
        [HttpGet("checkRbacWithAffiliation")]
        public IActionResult checkRbacWithAffiliation(string user, string aff, string roleName, string privilege, string permission)
        {
            var response = _enforcer.Enforce(user, aff, roleName, privilege, permission);
            return Ok(response);
        }

        //If there is no collection for example named “Container 3” then pls create it before inserting any new item in this collection by a particular user.

        [HttpGet("CreateContainerDataInsert")]
        public async Task<string> CreateContainerDataInsert(string user, string aff, string roleName, string privilege, string permission)
        {
            bool response = _enforcer.Enforce(user, aff, roleName, privilege, permission);
            if(response)
            {
                Database database = CreateCosmosClient();

                Container container3 = await database.CreateContainerIfNotExistsAsync(
                id: "Container3",
                partitionKeyPath: "/AccountNumber");

                //Get an existing user and permission.
                User user1 = database.GetUser(user);

                Employee emp = new Employee()
                {
                    Id = "emp1",
                    EmpName = "Emp",
                    AccountNumber = user
                };

                string result = await ValidateAllPermissionsForItem(endpoint,dbName,container3,user1,emp);
                return result;
               
            }
            else
            {
                return unauthorised;
            }
        }

        //If there is an existing collection for example named “Container 1” then pls insert new item in this collection by a particular user.

        [HttpGet("ExistingContainerDataInsert")]
        public async Task<string> ExistingContainerDataInsert(string user, string aff, string roleName, string privilege, string permission)
        {
            bool response = _enforcer.Enforce(user, aff, roleName, privilege, permission);
            if (response)
            {
                Database database = CreateCosmosClient();

                Container container1 = database.GetContainer("Container1");
                
                //Get an existing user and permission.
                User user2 = database.GetUser(user);

                Employee emp = new Employee()
                {
                    Id = "emp2",
                    EmpName = "Emp2",
                    AccountNumber = user
                };


                string result = await ValidateAllPermissionsForItem(endpoint, dbName, container1, user2, emp);
                return result;

            }
            else
            {
                return unauthorised;
            }
        }

        //Make sure that user who is creating or inserting the item, can only update or delete it based upon the permission and partition key.

        [HttpGet("ExistingContainerDataUpdate")]
        public async Task<string> ExistingContainerDataUpdate(string user, string aff, string roleName, string privilege, string permission)
        {
            bool response = _enforcer.Enforce(user, aff, roleName, privilege, permission);
            if (response)
            {
                Database database = CreateCosmosClient();

                Container container3 = database.GetContainer("Container3");

                //Get an existing user and permission.
                User user1 = database.GetUser(user);

                Employee emp = new Employee()
                {
                    Id = "emp1",
                    EmpName = "Emp",
                    AccountNumber = user
                };

                string result = await ValidateAllPermissions(endpoint, dbName, container3, user1, emp);
                return result;
            }
            else
            {
                return unauthorised;
            }
        }

        //If there is only “read” permission, then this item must only be readable by any other user and must not be updated / deleted vice versa to the “All” permission.

        [HttpGet("DifferentUserDataRead")]
        public async Task<string> DifferentUserDataRead(string user, string aff, string roleName, string privilege, string permission)
        {
            bool response = _enforcer.Enforce(user, aff, roleName, privilege, permission);
            if (response)
            {
                Database database = CreateCosmosClient();

                User user1 = database.GetUser(user);

                Container container3 = database.GetContainer("Container3");

                Employee emp = new Employee()
                {
                    Id = "emp1",
                    AccountNumber = "User1"
                };
                try
                {
                    if (user == "User2")
                    {
                        // Read Permission 
                        PermissionProperties readPermission = new PermissionProperties(
                        id: "permissionRead",
                        permissionMode: PermissionMode.Read,
                        container: container3,
                        resourcePartitionKey: new PartitionKey(emp.AccountNumber));

                        PermissionProperties readPermissionItem = await user1.UpsertPermissionAsync(readPermission);

                        // Create a new client with the generated token
                        using (CosmosClient permissionClient = new CosmosClient(endpoint, readPermissionItem.Token))
                        {
                            Container permissionContainer = permissionClient.GetContainer(dbName, container3.Id);

                            try
                            {
                                //Read the item
                                Employee item = await permissionContainer.ReadItemAsync<Employee>(emp.Id, new PartitionKey(emp.AccountNumber));
                                return JsonConvert.SerializeObject(item);

                            }
                            catch (CosmosException ce)
                            {
                                return ce.Message;
                            }
                        }
                        

                    }
                    else
                    {
                        return await GetUser1Data(user1, container3);
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return unauthorised;
            }
        }

        [HttpGet("DifferentUserDataUpdate")]
        public async Task<string> DifferentUserDataUpdate(string user, string aff, string roleName, string privilege, string permission)
        {
            bool response = _enforcer.Enforce(user, aff, roleName, privilege, permission);
            if (response)
            {
                Database database = CreateCosmosClient();

                User user1 = database.GetUser(user);

                Container container3 = database.GetContainer("Container3");

                Employee emp = new Employee()
                {
                    Id = "emp1",
                    AccountNumber = "User1"
                };
                try
                {
                    if (user == "User2")
                    {
                        // Read Permission 
                        PermissionProperties readPermission = new PermissionProperties(
                        id: "permissionRead",
                        permissionMode: PermissionMode.Read,
                        container: container3,
                        resourcePartitionKey: new PartitionKey(emp.AccountNumber));

                        PermissionProperties readPermissionItem = await user1.UpsertPermissionAsync(readPermission);

                        // Create a new client with the generated token
                        using (CosmosClient permissionClient = new CosmosClient(endpoint, readPermissionItem.Token))
                        {
                            Container permissionContainer = permissionClient.GetContainer(dbName, container3.Id);

                            try
                            {
                                //Read the item
                                Employee item = await permissionContainer.ReadItemAsync<Employee>(emp.Id, new PartitionKey(emp.AccountNumber));

                                //try to update an item
                                item.AccountNumber = "User2";
                                await permissionContainer.UpsertItemAsync<Employee>(item);
                                return JsonConvert.SerializeObject(item);

                            }
                            catch (CosmosException ce)
                            {
                                return ce.Message;
                            }
                        }


                    }
                    else
                    {
                        return await GetUser1Data(user1, container3);
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return unauthorised;
            }
        }

        private async Task<string> GetUser1Data(User user, Container container3)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.AccountNumber = '{user}'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Employee> queryResultSetIterator = container3.GetItemQueryIterator<Employee>(queryDefinition);

            List<Employee> employees = new List<Employee>();

            while (queryResultSetIterator.HasMoreResults)
            {
                Microsoft.Azure.Cosmos.FeedResponse<Employee> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Employee employee in currentResultSet)
                {
                    employees.Add(employee);
                }

            }
            return JsonConvert.SerializeObject(employees);
        }
        private Database CreateCosmosClient()
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException("Please specify a valid endpoint");
            }

            if (string.IsNullOrEmpty(accountKey) || string.Equals(accountKey, "Secret Key"))
            {
                throw new ArgumentException("Please specify a valid AuthorizationKey");
            }

            CosmosClient client = new CosmosClient(endpoint, accountKey);

            string dbName = _configuration.GetValue<string>("UserDB");

            Database database = client.GetDatabase(dbName);

            return database;
        }

        // <ValidateAllPermissionsForItem>
        private static async Task<string> ValidateAllPermissionsForItem(
            string endpoint,
            string databaseName,
            Container container,
            User user,
            Employee emp)
        {
            // Create a permission on a container and specific partition key value
            PermissionProperties allPermissionForItem = new PermissionProperties(
                id: "permissionUserEmployee",
                permissionMode: PermissionMode.All,
                container: container,
                resourcePartitionKey: new PartitionKey(emp.AccountNumber));

            PermissionProperties allItemPermission = await user.UpsertPermissionAsync(allPermissionForItem);

            // Create a new client with the generated token
            using (CosmosClient cosmoClient = new CosmosClient(endpoint, allItemPermission.Token))
            {
                Container permissionContainer = cosmoClient.GetContainer(databaseName, container.Id);

                try
                {

                    // Write emp item
                    await permissionContainer.UpsertItemAsync<Employee>(
                        emp,
                        new PartitionKey(emp.AccountNumber));
                    return $"Upsert item in {container.Id} with all permission succeeded.";
                }
                catch(Exception exp)
                {
                    return exp.Message;
                }
            }
        }
        // </ValidateAllPermissionsForItem>

        
        private static async Task<string> ValidateAllPermissions(
            string endpoint,
            string databaseName,
            Container container,
            User user,
            Employee emp)
        {
            // Create a permission on a container and specific partition key value
            PermissionProperties allPermissionForItem = new PermissionProperties(
                id: "permissionUserEmployee",
                permissionMode: PermissionMode.All,
                container: container,
                resourcePartitionKey: new PartitionKey(emp.AccountNumber));

            PermissionProperties allItemPermission = await user.UpsertPermissionAsync(allPermissionForItem);

            // Create a new client with the generated token
            using (CosmosClient cosmoClient = new CosmosClient(endpoint, allItemPermission.Token))
            {
                Container permissionContainer = cosmoClient.GetContainer(databaseName, container.Id);

                try
                {
                    if (emp.AccountNumber == "User1")
                    {
                        Employee reademp = new Employee();
                        reademp = await permissionContainer.ReadItemAsync<Employee>(
                                emp.Id,
                                new PartitionKey(emp.AccountNumber));

                        reademp.EmpName = "UpdatedEmp";
                        await permissionContainer.UpsertItemAsync<Employee>(
                        reademp,
                        new PartitionKey(emp.AccountNumber));
                        return $"update and delete that item from {container.Id} with all permission succeeded.";
                    }
                    return "User don't have permission to update or delete an item";
                }
                catch (Exception exp)
                {
                    return exp.Message;
                }
            }
        }
        

    }
}

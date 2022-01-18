using NetCasbin.Rbac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasbinRBAC.Domain.Models
{
    public class RoleManager : IRoleManager
    {
        public Func<string, string, bool> MatchingFunc { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Func<string, string, bool> DomainMatchingFunc { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool HasPattern => throw new NotImplementedException();

        public bool HasDomainPattern => throw new NotImplementedException();

        public void AddLink(string name1, string name2, params string[] domain)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void DeleteLink(string name1, string name2, params string[] domain)
        {
            throw new NotImplementedException();
        }

        public List<string> GetRoles(string name, params string[] domain)
        {
            throw new NotImplementedException();
        }

        public List<string> GetUsers(string name, params string[] domain)
        {
            throw new NotImplementedException();
        }

        public bool HasLink(string name1, string name2, params string[] domain)
        {
            throw new NotImplementedException();
        }
    }
}

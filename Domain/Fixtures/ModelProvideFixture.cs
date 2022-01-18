using NetCasbin.Model;
using System.IO;
namespace CasbinRBAC.Domain.Fixtures
{
    public class ModelProvideFixture
    {
        private readonly string _rbacModelText = File.ReadAllText("CasbinConfig/rbac_model.conf");

        public Model GetNewRbacModel()
        {
            return Model.CreateDefaultFromText(_rbacModelText);
        }
    }
}

using CasbinRBAC.Domain.Abstractions;
using NetCasbin.Model;
using NetCasbin.Persist;
using System.Collections.Generic;

namespace CasbinRBAC.Domain.Extensions
{
    public static class CasbinModelExtension
    {
        internal static void LoadPolicyFromCasbinRules<TCasbinRule>(this Model casbinModel, IEnumerable<TCasbinRule> rules)
            where TCasbinRule : class, ICasbinRule
        {
            foreach (TCasbinRule rule in rules)
            {
                string ruleString = rule.ConvertToString();
                Helper.LoadPolicyLine(ruleString, casbinModel);
            }
        }
    }
}

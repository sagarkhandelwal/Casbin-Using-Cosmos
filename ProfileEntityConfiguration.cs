using CasbinRBAC.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCasbin
{
    public class ProfileEntityConfiguration : IEntityTypeConfiguration<CasbinRule<int>>
    {
        public void Configure(EntityTypeBuilder<CasbinRule<int>> builder)
        {
            builder.HasKey(x => x.Id);
            //builder.HasPartitionKey(x => x.Id);

        }
    }
}

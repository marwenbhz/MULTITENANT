using System;
using DOMAIN.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace DATA.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {

        public UserConfiguration()
        {
            



            HasOptional(j => j.team)
            .WithMany(o => o.Users)
            .HasForeignKey(j => j.TeamCode)
            .WillCascadeOnDelete(false);

            HasOptional(j => j.tenant)
            .WithMany(o => o.Users)
            .HasForeignKey(j => j.TenantCode)
            .WillCascadeOnDelete(false);


        }

    }
}

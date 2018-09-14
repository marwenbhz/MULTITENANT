using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Configurations
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
                        
            //ManyToOne (Project,company)

            HasOptional(j => j.company)
            .WithMany(o => o.Projects)
            .HasForeignKey(j => j.CompanyCode)
            .WillCascadeOnDelete(false);

            //ManyToOne (Project, category)

            HasOptional(j => j.category)
             .WithMany(o => o.Projects)
            .HasForeignKey(j => j.CategoryCode)
            .WillCascadeOnDelete(false);

            //ManyToOne (Project, banque)

            HasOptional(j => j.banque)
           .WithMany(o => o.MyProjects)
           .HasForeignKey(j => j.BanqueCode)
        .WillCascadeOnDelete(false);

            //OneToMany (Project, Task)

            HasMany(j => j.Tasks)
                .WithRequired(o => o.project)
                .HasForeignKey(x => x.ProjectCode)
                .WillCascadeOnDelete(true);
                        
        }
    }
}

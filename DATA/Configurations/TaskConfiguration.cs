using System;
using DOMAIN.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace DATA.Configurations
{
    public class TaskConfiguration : EntityTypeConfiguration<DOMAIN.Entities.Task>
    {

        public TaskConfiguration()
        {
            /*

            //OneToMany (Task, projects)

            HasRequired(j => j.project)
            .WithMany(o => o.Tasks)
            .HasForeignKey(j => j.ProjectCode)
            .WillCascadeOnDelete(false);

    */

            //ManyToMany (Task, Users)

            HasMany(o => o.TeamMembers).WithOptional(j => j.MemberTask).HasForeignKey(t => t.TaskCode);
            HasOptional(w => w.TeamLeader).WithMany(x => x.LeaderTasks).HasForeignKey(y => y.TeamLeaderCode);


        }

    }
}

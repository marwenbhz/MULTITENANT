using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string Owner { get; set; }

        public string FullName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        public Countries Country { get; set; }

        public string UPN { get; set; }
                
        public string Username { get; set; }

        public UserType UserType { get; set; }

        public string TenantID { get; set; }

        public int? TenantCode { get; set; }

        public int? TeamCode { get; set; }

        public int? TaskCode { get; set; }

        [ForeignKey("TeamCode")]
        public virtual Team team { get; set; }

        [ForeignKey("TenantCode")]
        public virtual Tenant tenant { get; set; }

        [ForeignKey("TaskCode")]
        public virtual Task MemberTask { get; set; }

        public virtual ICollection<Task> LeaderTasks  { get; set; }

    }

}
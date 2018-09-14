using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Tenant
    {
        public int ID { get; set; }
        public string IssValue { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        [DisplayName("Check this if you are an administrator and you want to enable the app for all your users")]
        public bool AdminConsented { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}

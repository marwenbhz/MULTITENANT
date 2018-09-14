using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Banque
    {
        public int ID { get; set; }
        public string Owner { get; set; }
                
        [Display(Name = "Financed by | Banque")]
        public string BanqueName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Project> MyProjects { get; set; }
    }
}

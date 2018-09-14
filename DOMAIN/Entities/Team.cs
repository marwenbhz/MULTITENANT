using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Team
    {

        public int TeamID { get; set; }

        public string Owner { get; set; }

        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Display(Name = "Team Description")]
        public string TeamDescription { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
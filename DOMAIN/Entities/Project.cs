using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Project
    {

        public int ProjectID { get; set; }

        public string Owner { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Category")]
        public int? CategoryCode { get; set; }

        [Display(Name = "Company")]
        public int? CompanyCode { get; set; }

        [Display(Name = "Banque ")]
        public int? BanqueCode { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Plan")]
        [DataType(DataType.MultilineText)]
        public string Plan { get; set; }

        [Display(Name = "Goals")]
        [DataType(DataType.MultilineText)]
        public string Goals { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Deadline")]
        [DataType(DataType.Date)]
        public DateTime DeadLine { get; set; }

        [Display(Name = "Estimated Time")]
        public string EstimatedTime { get; set; }

        [ForeignKey("CompanyCode")]
        public virtual Company company { get; set; }

        [ForeignKey("CategoryCode")]
        public virtual Category category { get; set; }

        [ForeignKey("BanqueCode")]
        public virtual Banque banque { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Task
    {
        public int TaskID { get; set; }

        public string Owner { get; set; }

        public int ProjectCode { get; set; }

        public int? TeamLeaderCode { get; set; }

        [Display(Name = "Task Name")]
        public string TaskName { get; set; }


        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Plan")]
        [DataType(DataType.MultilineText)]
        public string Plan { get; set; }

        [Display(Name = "Goals")]
        [DataType(DataType.MultilineText)]
        public string Goals { get; set; }

        [Display(Name = "Requirement")]
        [DataType(DataType.MultilineText)]
        public string Requirement { get; set; }

        [Display(Name = "Tools")]
        [DataType(DataType.MultilineText)]
        public string Tools { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Deadline")]
        [DataType(DataType.Date)]
        public DateTime DeadLine { get; set; }

        [Display(Name = "Estimated Time")]
        public string EstimatedTime { get; set; }

        public ComplexityEnum Complexity { get; set; }

        public StateEnum State { get; set; }

        [ForeignKey("TeamLeaderCode")]
        public virtual User TeamLeader { get; set; }

        public virtual ICollection<User> TeamMembers { get; set; }

        [ForeignKey("ProjectCode")]
        public virtual Project project { get; set; }
       
    }
}

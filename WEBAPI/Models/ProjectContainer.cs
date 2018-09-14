using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI.Models
{
    public class ProjectContainer
    {
        public int ProjectID { get; set; }
                         
        public string ProjectName { get; set; }
        public string Category { get; set; }
           
        public string Company { get; set; }
          
        public string Banque { get; set; }
        public string Description { get; set; }
        public string Plan { get; set; }
        public string Goals { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadLine { get; set; }
        public string EstimatedTime { get; set; }

    }
}
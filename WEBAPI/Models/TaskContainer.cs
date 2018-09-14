using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI.Models
{
    public class TaskContainer
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string Plan { get; set; }
        public string Goals { get; set; }
        public string Requirement { get; set; }
        public string Tools { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadLine { get; set; }
        public string EstimatedTime { get; set; }
        public string ComplexityString { get; set; }
        public string StateString { get; set; }
        public string Project { get; set; }
        public string TeamLeader { get; set; }

    }
}
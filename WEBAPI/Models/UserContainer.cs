using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPI.Models
{
    public class UserContainer
    {
        public int UserID { get; set; }
               
        public string FullName { get; set; }

        public string Bio { get; set; }

        public string Country { get; set; }

        public string UPN { get; set; }

        public string Username { get; set; }

        public string UserType { get; set; }

        public string TeamName { get; set; }

        public string TaskName { get; set; }
    }
}
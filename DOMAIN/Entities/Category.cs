using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Category
    {

        public int CategoryID { get; set; }

        public string Owner { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string CategoryDescription { get; set; }

        public virtual ICollection<Project> Projects { get; set; }


    }
}
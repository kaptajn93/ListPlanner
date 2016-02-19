using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListPlanner.Models
{
    public class ListItem
    {

        [ScaffoldColumn(false)]
        public int ItemID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public int Parent { get; set; }

        public bool IsDone { get; set; }

        [ScaffoldColumn(false)]
        public int ListID { get; set; }

   //     public virtual ToDoList ToDoList { get; set; }
    }
}

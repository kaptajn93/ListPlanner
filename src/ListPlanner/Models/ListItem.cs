using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ListPlanner.Models
{
    public class ListItem
    {

        [ScaffoldColumn(false)]
        public int ListItemID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string ItemName { get; set; }

        public int Parent { get; set; }

        public bool IsDone { get; set; }

        [ScaffoldColumn(false)]
        public int ToDoListID { get; set; }

        public virtual ToDoList ToDoList { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListPlanner.Models
{
    public class ToDoList
    {
        [ScaffoldColumn(false)]
        public int ToDoListID { get; set; }

        [Required]
        public string Title { get; set; }

        public ICollection<ListItem> Items { get; set; }

        public Boolean Selected { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public int UserID { get; set; }

        
        // Navigation property
        public virtual User User { get; set; }

     
    }
}

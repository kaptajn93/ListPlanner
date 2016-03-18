using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ListPlanner.Models
{
    public class User
    {
        [ScaffoldColumn(false)]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Alias { get; set; }
      
        public virtual ICollection<ToDoList> Lists { get; set; }
    }


    public class CacheEntry
    {
        public string Key { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

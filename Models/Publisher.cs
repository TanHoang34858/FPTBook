using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBook.Models
{
    public class Publisher
    {
        [Display(Name = "Publisher code")]
        public int ID { get; set; }
        [Required(ErrorMessage = "This value cannot be left blank")]
        [Display(Name = "Publisher code")]
        public string Name { get; set; }
        [Display(Name = "Describe")]
        public string Description { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

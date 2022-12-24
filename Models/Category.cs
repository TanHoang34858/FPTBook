using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IBook.Models
{
    public class Category
    {
        [Display(Name = "CategoryID")]
        public int ID { get; set; }
        [Required(ErrorMessage = "This value cannot be left blank")]
        [Display(Name = "NameID")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

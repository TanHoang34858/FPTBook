using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBook.Models
{
    public class Author
    {
        [Display(Name = "Author ID")]
        public int ID { get; set; }
        [Required(ErrorMessage = "This value cannot be left blank")]
        [Display(Name = "Name Author")]
        public string Name { get; set; }
        [Display(Name = "Story")]
        public string History { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

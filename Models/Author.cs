using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBook.Models
{
    public class Author
    {
        [Display(Name = "Mã tác giả")]
        public int ID { get; set; }
        [Required(ErrorMessage = "Giá trị này không được bỏ trống")]
        [Display(Name = "Tên tác giả")]
        public string Name { get; set; }
        [Display(Name = "Tiểu sử")]
        public string History { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

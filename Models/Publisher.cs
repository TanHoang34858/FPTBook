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
        [Display(Name = "Mã nhà xuất bản")]
        public int ID { get; set; }
        [Required(ErrorMessage = "Giá trị này không được bỏ trống")]
        [Display(Name = "Mã nhà xuất bản")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

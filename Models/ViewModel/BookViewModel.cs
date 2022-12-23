using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBook.Models.ViewModel
{
    public class BookViewModel
    {
        public Book book { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Author> authors { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBook.Models.ViewModel
{
    public class ShoppingCartViewModel
    {
        public List<Book> books { get; set; }
        public OrderBook orderBook { get; set; }
    }
}

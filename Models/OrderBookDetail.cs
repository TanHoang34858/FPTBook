using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBook.Models
{
    public class OrderBookDetail
    {
        [Display(Name = "Order Detail ID")]
        [ForeignKey("OrderBook")]
        public int OrderBookID { get; set; }

        [Display(Name = "Book code")]
        [ForeignKey("Book")]
        public int BookID { get; set; }

        [Display(Name = "Book title")]
        public virtual Book Book { get; set; }

        [Display(Name = "Code orders")]
        public virtual OrderBook OrderBook { get; set; }
    }
}

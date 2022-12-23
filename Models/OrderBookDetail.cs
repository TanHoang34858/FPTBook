using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBook.Models
{
    public class OrderBookDetail
    {
        [Display(Name = "Mã chi tiết đơn hàng")]
        [ForeignKey("OrderBook")]
        public int OrderBookID { get; set; }

        [Display(Name = "Mã sách")]
        [ForeignKey("Book")]
        public int BookID { get; set; }

        //[Display(Name = "Số lượng")]
        //public int Quantities { get; set; }

        [Display(Name = "Tên sách")]
        public virtual Book Book { get; set; }

        [Display(Name = "Mã đơn hàng")]
        public virtual OrderBook OrderBook { get; set; }
    }
}

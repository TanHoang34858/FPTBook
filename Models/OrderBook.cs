using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IBook.Models
{ 
    public class OrderBook
    {
        [Display(Name = "Mã đơn hàng")]
        public int OrderID { get; set; }

        [Display(Name = "Ngày giao hàng")]
        public DateTime AppointmentDate { get; set; }

        [NotMapped]
        [Display(Name = "Giờ giao hàng")]
        public DateTime AppointmentTime { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Số điện thoại liên hệ")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Xác nhận")]
        public Boolean isConfirmed { get; set; }
        public virtual ICollection<OrderBookDetail> OrderBookDetails { get; set; }
    }
}

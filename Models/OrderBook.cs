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
        [Display(Name = "Code orders")]
        public int OrderID { get; set; }

        [Display(Name = "Delivery date")]
        public DateTime AppointmentDate { get; set; }

        [NotMapped]
        [Display(Name = "Delivery time")]
        public DateTime AppointmentTime { get; set; }

        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }

        [Display(Name = "Delivery address")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Contact phone number")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Confirm")]
        public Boolean isConfirmed { get; set; }
        public virtual ICollection<OrderBookDetail> OrderBookDetails { get; set; }
    }
}

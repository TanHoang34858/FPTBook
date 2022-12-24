using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBook.Models
{
    public class Book
    {
        public int BookID { get; set; }

        [Display(Name = "Book name")]
        public string Title { get; set; }

        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Display(Name = "Quantity")]
        public int Quantities { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Publication date")]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Reprint date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Number of pages")]
        public int NumberOfPages { get; set; }

        [Display(Name = "Price")]
        public Int64 Price { get; set; }

        [Display(Name = "Cover image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Permission to sell")]
        public Boolean IsPurchase { get; set; }

        [Display(Name = "Author")]
        public int AuthorID { get; set; }

        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Publisher")]
        public int PublisherID { get; set; }

        [Display(Name = "Author")]
        public virtual Author Author { get; set; }

        [Display(Name = "Category")]
        public virtual Category Category { get; set; }

        public virtual ICollection<OrderBookDetail> OrderBookDetails { get; set; }

        [Display(Name = "Author")]
        public virtual Publisher Publisher { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Prosiak.Areas.Books
{
    public class Book
    {
        public int ID { get; set; }
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        
        [StringLength(60)]
        public string Title { get; set; }
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Author { get; set; }
        [MinLength(10), MaxLength(13)]
        [Display(Name="ISBN")]
        public string Isbn { get; set; }
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Category { get; set; }
        [Required]
        [StringLength(30)]
        public string Owner { get; set; }
        [StringLength(30)]
        public string Reader { get; set; }
        [Display(Name = "Active Flag")]
        public bool Status { get; set; }
        [Display(Name="Last Edit Date")]
        public DateTime Timestamp { get; set; }
        [Display(Name="Last Edited by")]
        public string UsrStamp { get; set; }
    }

    public class BookCard
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BorrowDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnDate { get; set; }
        public string Reader { get; set; }

    }

    public class BooksDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCard> Cards { get; set; }
    }
}
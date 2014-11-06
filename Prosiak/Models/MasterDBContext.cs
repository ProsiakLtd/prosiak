using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Prosiak.Areas.Books;

namespace Prosiak.Models
{
    public class MasterDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCard> Cards { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Prosiak.Areas.Books;

//Create your DBContexts in this folder
//To Create new database, or Add tables to an already existing database, run the following commands:
//Enable-Migrations -ContextTypeName BooksDBContext -MigrationsDirectory  DataContexts\BooksMigrations
//Note: Store migrations for each DBContext in a separate folder
//Add-Migration -ConfigurationTypeName Prosiak.DataContexts.BooksMigrations.Configuration "InitialCreate"
//Update-Database -ConfigurationTypeName Prosiak.DataContexts.BooksMigrations.Configuration
//remember to specify the same connection string for all contexts to keep all the tables inside one DB
namespace Prosiak.DataContexts
{
    
    public class BooksDBContext : DbContext
    {
        public BooksDBContext() : base("database") { }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCard> Cards { get; set; }
    }

}
namespace Prosiak.DataContexts.BooksMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using Prosiak.Areas.Books;

    internal sealed class Configuration : DbMigrationsConfiguration<Prosiak.DataContexts.BooksDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataContexts\BooksMigrations";
        }

        protected override void Seed(Prosiak.DataContexts.BooksDBContext context)
        {
                context.Books.AddOrUpdate(b=>b.Title,
                    new Book() { Author = "James Peterson", Title = "Double Cross", Isbn = "9780755330331", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Peter James", Title = "Dead Simple", Isbn = "0330546015", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Lynda La Plante", Title = "Clean Cut", Isbn = "9781416594512", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Piotr C Kowalski", Title = "Pokolenie Ikea", Isbn = "8377229285", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Harlan Coben", Title = "Miracle Cure", Isbn = "1101544449", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Harlan Coben", Title = "Long Lost", Isbn = "1101028742", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Harlan Coben", Title = "Black Spin", Isbn = "1409119750", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Harlan Coben", Title = "Live Wire", Isbn = "1101476168", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Stephen King", Title = "Pod Kopu³¹", Isbn = "1439148503", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Carlos Ruiz Zafon", Title = "The Shadow of the Wind", Isbn = "0297857134", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "John le Carre", Title = "A Small Town in Germany", Isbn = "1101603046", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Stephen Leather", Title = "Cold Kill", Isbn = "1844568555", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "David Baldacci", Title = "Hour Game", Isbn = "0330506846", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Land of Fire", Title = "Chris Ryan", Isbn = "1409035409", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Terence Strong", Title = "White Viper", Isbn = "1847390331", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "James Twining", Title = "The Double Eagle", Isbn = "0007389582", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "John le Carre", Title = "The Little Drummer Girl", Isbn = "1101535334", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Scott Mariani", Title = "Shadow Project, The", Isbn = "0007358024", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Sam Christer", Title = "The Stonehenge Legacy", Isbn = "0748123601", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Robert Harris", Title = "Enigma", Isbn = "1409021599", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" },
                    new Book() { Author = "Catherine Cookson", Title = "The Silent Lady", Isbn = "0743230035", Owner = "unknown", Category = Areas.Books.Models.BookGenre.Literature, Status = true, Timestamp = DateTime.Now, UsrStamp = "flism" }
                    );
        }
    }
}

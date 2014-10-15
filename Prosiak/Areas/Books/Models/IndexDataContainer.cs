using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prosiak.Areas.Books
{
    public class IndexDataContainer
    {
        public int PageNum { get; set; }
        public int ChunkSize { get; set; }
        public string Category { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
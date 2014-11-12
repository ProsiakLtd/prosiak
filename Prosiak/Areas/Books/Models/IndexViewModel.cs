using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prosiak.Areas.Books
{
    public class IndexViewModel
    {
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public string Category { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public SelectList ResultsPerPageOptions { get; set; }

        public IndexViewModel(int page, int resultsPerPage, string category, string searchString, IEnumerable<Book> books)
        {
            Page = page;
            ResultsPerPage = resultsPerPage;
            Category = category;
            SearchString = searchString;
            Books = books;
        }
        public IndexViewModel() { }
    }
}
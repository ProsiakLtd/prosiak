namespace Prosiak.Areas.Books.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using PagedList;
    using Areas.Books.Helpers;
    
    using System.IO;

    /// <summary>
    /// Controller for Book Area
    /// </summary>
    public class BooksController : Controller
    {
        private BooksDBContext db = new BooksDBContext();
        //private IEnumerable<IEnumerable<Book>> booksGrouped;

        public ActionResult Autocomplete(string term)
        {
            var model = db.Books
                .Where(b => b.Title.ToLower().StartsWith(term.ToLower()))
                .DistinctBy(t => t.Title)
                .Select(s => s.Title)
                .Union(db.Books
                    .Where(b => b.Author.StartsWith(term.ToLower()))
                    .DistinctBy(t => t.Author)
                    .Select(s => s.Author)
                )
                .OrderBy(s=>s)
                .Select(hint=>new { label = hint });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GET: /Books/
        public ActionResult Index(string category, string searchString, int resultsPerPage = 10, int page = 1)
        {
            db.Database.CreateIfNotExists();
            
            var books = db.Books.Where(b=>b.Status == true).ToList();

            var model = BuildIndexViewModel(category, searchString, resultsPerPage, page, books);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Books", model);
            }

            return View(model);
        }

        /// <summary>
        /// This either borrows or returns the book
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int bookId, string category, string searchString, int resultsPerPage = 10, int page = 1)
        {
            var book = db.Books.Single(b => b.ID == bookId);
            BookCard bc;
            bool conflict = false;

            //no one is reading it? borrow!
            if (book.Reader == null){
                    bc = new BookCard
                    {
                        BookID = bookId,
                        BorrowDate = DateTime.Now,
                        Reader = User.Identity.Name
                    }; 
                    book.Reader = User.Identity.Name;
                    db.Cards.Add(bc);
                    db.Entry(book).State = EntityState.Modified;
            }
            //user is the reader? return!
            else if (book.Reader == User.Identity.Name)
            {
                bc = db.Cards.Single(c=>c.BookID == bookId && c.ReturnDate == null);

                bc.ReturnDate = DateTime.Now;
                book.Reader = null;
                db.Entry(bc).State = EntityState.Modified;
                db.Entry(book).State = EntityState.Modified;
            } 
            else //this will probably only happen if the book was borrowed by someone else 
                //after the page was loaded but before the button click
            {
                conflict = true;
            }

            db.SaveChanges();

            if (Request.IsAjaxRequest())
            {
                return Content((HtmlHelpers.GetButton(bookId, book.Reader, User.Identity.Name, conflict)).ToHtmlString());
            }

            var books = db.Books.Where(b => b.Status == true).ToList();

            var model = BuildIndexViewModel(category, searchString, resultsPerPage, page, books);
            
            return View(model);
        }

        

        // GET: /Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // GET: /Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Author,Isbn,Category,Owner,Status")] Book book)
        {
            book.Timestamp = DateTime.Now;
            book.UsrStamp = User.Identity.Name;
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: /Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // POST: /Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Author,Isbn,Category,Owner,Status")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.Timestamp = DateTime.Now;
                book.UsrStamp = User.Identity.Name;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        [HttpPost]
        public ActionResult FetchThumbnail(int bookId)
        {
            var book = db.Books.Single(b => b.ID == bookId);
            var thumbnailPath = Helpers.Thumbnails.GetThumbnailDiskPath(book.Isbn);
            if (System.IO.File.Exists(thumbnailPath))
            {
                return RedirectToAction("Edit", new { id = bookId });
            }
            else
            {
                string thumbnailUrl = Helpers.Thumbnails.TryGetThumbnailFromGoogle(book.Isbn);
                if (!string.IsNullOrEmpty(thumbnailUrl))
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(thumbnailUrl, thumbnailPath);
                    }
                }
                return RedirectToAction("Edit", new { id = bookId });
            }
            //return RedirectToAction("Edit", new { id = bookId });
        }

        // GET: /Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // POST: /Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }

        private IndexViewModel BuildIndexViewModel(string category, string searchString, int resultsPerPage, int page, IEnumerable<Book> books)
        {
            var categoryList = new List<string>();
            categoryList.AddRange(books.OrderBy(b=>b.Category).Select(b=>b.Category).Distinct());

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            if (!(String.IsNullOrEmpty(category) || category == "All"))
            {
                books = books.Where(b => b.Category == category);
            }

            var pagedBooks = books.ToPagedList(page, resultsPerPage);

            IndexViewModel model = new IndexViewModel(page, resultsPerPage, category, searchString, pagedBooks);
            model.ResultsPerPageOptions = new SelectList(new List<string> { "10", "20", "50" });
            model.Categories = new SelectList(categoryList);

            return model;
        }
       
    }
}
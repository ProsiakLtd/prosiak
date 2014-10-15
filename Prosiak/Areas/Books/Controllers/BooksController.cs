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

    /// <summary>
    /// Controller for Book Area
    /// </summary>
    public class BooksController : Controller
    {
        private BooksDBContext db = new BooksDBContext();
        //private IEnumerable<IEnumerable<Book>> booksGrouped;

        // GET: /Books/
        public ActionResult Index(string bookCategory, string searchString, string showNum, int? resPage)
        {
            db.Database.CreateIfNotExists();
            var categoryList = new List<string>();

            var categoryQuery = from d in db.Books
                                orderby d.Category
                                select d.Category;
            categoryList.AddRange(categoryQuery.Distinct());
            ViewBag.bookCategory = new SelectList(categoryList);
            ViewBag.showNum = new SelectList(new List<string> { "10", "20", "50" });
            ViewBag.selectedShowNum = showNum;
            ViewBag.resPage = resPage != null ? resPage - 1 : 0;
            var books = from b in db.Books
                        where b.Status == true
                        select b;

            var booksList = (IEnumerable<Book>)books.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                booksList = booksList.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            if (!(String.IsNullOrEmpty(bookCategory) || bookCategory == "All"))
            {
                booksList = booksList.Where(b => b.Category == bookCategory);
            }


            var pagedBooks = booksList.ToPagedList(resPage ?? 1, int.Parse(showNum ?? "10"));
            IndexDataContainer model = new IndexDataContainer()
            {
                Category = bookCategory,
                SearchString = searchString,
                PageNum = resPage ?? 1,
                ChunkSize = showNum==null ? 10 : int.Parse(showNum),
                Books = pagedBooks
            };
            return View(model);
        }

        /// <summary>
        /// Post method for borrowing/returning
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="returnInd">1 if book is returned 0 if book is borrowed</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(int? bookid, int? returnInd, string bookCategory, string searchString, string showNum, int? resPage)
        {
            //this is required?
            var categoryList = new List<string>();

            var categoryQuery = from d in db.Books
                                orderby d.Category
                                select d.Category;
            categoryList.AddRange(categoryQuery.Distinct());
            ViewBag.bookCategory = new SelectList(categoryList);
            ViewBag.showNum = new SelectList(new List<string> { "10", "20", "50" });
            var books = from b in db.Books
                        where b.Status == true
                        select b;

            var booksList = (IEnumerable<Book>)books.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                booksList = booksList.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            if (!(String.IsNullOrEmpty(bookCategory) || bookCategory == "All"))
            {
                booksList = booksList.Where(b => b.Category == bookCategory);
            }

            var pagedBooks = booksList.ToPagedList(resPage ?? 1, int.Parse(showNum ?? "10"));
            IndexDataContainer model = new IndexDataContainer()
            {
                Category = bookCategory,
                SearchString = searchString,
                PageNum = resPage ?? 1,
                ChunkSize = showNum == null ? 10 : int.Parse(showNum),
                Books = pagedBooks
            };


            if (ModelState.IsValid && bookid != null && returnInd != null)
            {
                BookCard bc;
                Book b = (from d in db.Books
                          where d.ID == bookid
                          select d).First();

                b.Timestamp = DateTime.Now; //should I update this?
                b.UsrStamp = User.Identity.Name; //-||-

                switch (returnInd)
                {
                    case 0://borrow
                        bc = new BookCard
                        {
                            BookID = (int)bookid,
                            BorrowDate = DateTime.Now,
                            Reader = User.Identity.Name
                        }; //CAST!!!
                        b.Reader = User.Identity.Name;
                        db.Cards.Add(bc);
                        break;
                    case 1: //return
                        bc = (from c in db.Cards
                              where c.BookID == bookid && c.ReturnDate == null

                              select c).First();
                        bc.ReturnDate = DateTime.Now;
                        b.Reader = null;
                        db.Entry(bc).State = EntityState.Modified;
                        break;
                }

                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    resPage = model.PageNum,
                    bookCategory = model.Category,
                    searchString = model.SearchString,
                    showNum = model.ChunkSize
                }
                );

                //return RedirectToAction("Index", new { bookCategory = this.bookCategory, searchString = searchString, showNum = showNum, resPage = resPage });
            }
   
            //below should never ever ever happen?
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
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Prosiak.Areas.Books.Controllers
{
    public class BooksController : Controller
    {
        private BooksDBContext db = new BooksDBContext();
        IEnumerable<IEnumerable<Book>> booksGrouped;
        // GET: /Books/
        public ActionResult Index(string bookCategory, string searchString, string showNum, int? resPage)
        {
            db.Database.CreateIfNotExists();
            var CategoryList = new List<string>();

            var CategoryQuery = from d in db.Books
                                orderby d.Category
                                select d.Category;
            CategoryList.AddRange(CategoryQuery.Distinct());
            ViewBag.bookCategory = new SelectList(CategoryList);
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

            if (!String.IsNullOrEmpty(bookCategory))
            {
                booksList = booksList.Where(b => b.Category == bookCategory);
            }

            if (showNum == "All") //don't group
            {
                booksGrouped = (IEnumerable<IEnumerable<Book>>)new List<List<Book>>() { booksList.ToList() };
            }
            else if (!string.IsNullOrEmpty(showNum))
            {

                int groupsize = int.Parse(showNum);
                //doesnt work with entity
                //var ZZZ = books.Select((x, i) => new { Index = i, Value = x }).GroupBy(b => (b.Index - 1) / groupsize).Select(g => g.Select(v=>v.Value).ToList()).ToList();
                //var ZZZ = books.GroupBy((b) => (i - 1) / groupsize).Select(g => g.ToList()).ToList();
                GroupBooks( ref booksGrouped, booksList, groupsize);
                //booksGrouped = (IEnumerable<IEnumerable<Book>>)new List<List<Book>>() { books.ToList() };
            }
            else
                booksGrouped = (IEnumerable<IEnumerable<Book>>)new List<List<Book>>() { books.ToList() };
            return View(booksGrouped);
        }

        private void GroupBooks(ref IEnumerable<IEnumerable<Book>> booksGrouped, IEnumerable<Book> books, int chunksize)
        {
            var books2 = books.ToList();
            var x = from book in books2
                    let index = books2.IndexOf(book)
                    group book by (index + 1) / chunksize into g
                    select g.AsEnumerable();
            booksGrouped = x;
        }

        /// <summary>
        /// Post method for borrowing/returning
        /// </summary>
        /// <param name="Bookid"></param>
        /// <param name="Return">1 if book is returned 0 if book is borrowed</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(int? Bookid, int? Return)
        {
            //this is required?
            var CategoryList = new List<string>();

            var CategoryQuery = from d in db.Books
                                orderby d.Category
                                select d.Category;
            CategoryList.AddRange(CategoryQuery.Distinct());
            ViewBag.bookCategory = new SelectList(CategoryList);
            ViewBag.showNum = new SelectList(new List<string> { "10", "20", "50" });
            var books = from b in db.Books
                        where b.Status == true
                        select b;
            
            if (ModelState.IsValid && Bookid != null && Return != null)
            {


                BookCard bc;
                Book b = (from d in db.Books
                          where d.ID == Bookid
                          select d).First();

                b.Timestamp = DateTime.Now; //should I update this?
                b.UsrStamp = User.Identity.Name; //-||-

                switch (Return)
                {
                    case (0)://borrow
                        bc = new BookCard
                        {
                            BookID = (int)Bookid,
                            BorrowDate = DateTime.Now,
                            Reader = User.Identity.Name
                        }; //CAST!!!
                        b.Reader = User.Identity.Name;
                        db.Cards.Add(bc);
                        break;
                    case (1): //return
                        bc = (from c in db.Cards
                              where c.BookID == Bookid && c.ReturnDate == null

                              select c).First();
                        bc.ReturnDate = DateTime.Now;
                        b.Reader = null;
                        db.Entry(bc).State = EntityState.Modified;
                        break;
                }

                //ReturnDate = DateTime.MinValue 

                db.Entry(b).State = EntityState.Modified;
                //db.SaveChanges();

                db.SaveChanges();
                return RedirectToAction("Index");
                //return RedirectToAction("Index", new { bookCategory = this.bookCategory, searchString = searchString, showNum = showNum, resPage = resPage });
            }

            return View(books);
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using laba9.Models;

// Дата в изменении/добавлении работает некорретно!!! Надо исправить выводимый формат.
// Дата в изменении/добавлении работает некорретно!!! Надо исправить выводимый формат.
// Дата в изменении/добавлении работает некорретно!!! Надо исправить выводимый формат.
// Еще нужно добавить изменение, добавление в журнал + аутентификацию и разграничение ролей... ААААААААААААААА

namespace laba9.Controllers
{
    public class HomeController : Controller
    {
        private epiclibraryEntities db = new epiclibraryEntities();
        public ActionResult Index()
        {
            ViewBag.date = new DateTime();
            DataSet ds = new DataSet();
            var outputs = db.Outputs;
            var readers = db.Readers;
            var books = db.Books;

            var col = new List<string>() { "ID", "ID Читателя", "ФИО Читателя" ,
                    "Год рожд. Чит-ля", "Пасспорт Чит-ля",  "ID Кн.", "Название Кн.", "Автор Кн.",
                "Дата издания", "Дата написания", "Дата выдачи", "Последний срок приема", "Дней до просрочки"};
            DataTable table = new DataTable("Outputs");

            foreach (var c in col)
                table.Columns.Add(c);

            foreach (var o in outputs)
            {
                var R = new Readers();
                foreach (var r in readers)
                    if (r.r_id == o.R_id)
                        R = r;
                var B = new Books();
                foreach (var b in books)
                    if (b.b_id == o.B_id)
                        B = b;
                table.Rows.Add(
                    o.o_id,
                    R.r_id,
                    R.r_fio,
                    R.r_dt_birth,
                    R.r_passport,
                    B.b_id,
                    B.b_name,
                    B.b_author,
                    B.b_publ,
                    B.b_born,
                    o.o_dt_out,
                    o.o_dt_in,
                    o.o_dt_in - o.o_dt_out
                    );
            }

            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }

            ds.Tables.Add(table);

            return View(ds.Tables["Outputs"]);
        }

        public ActionResult Readers()
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            return View(db.Readers.ToList());
        }

        public ActionResult Books()
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            return View(db.Books.ToList());
        }

        public ActionResult Outputs()
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            ViewBag.date = new DateTime();
            DataSet ds = new DataSet();
            var outputs = db.Outputs;
            var readers = db.Readers;
            var books = db.Books;

            var col = new List<string>() { "ID", "ID Читателя", "ФИО Читателя" ,
                    "Год рожд. Чит-ля", "Пасспорт Чит-ля",  "ID Кн.", "Название Кн.", "Автор Кн.",
                "Дата издания", "Дата написания", "Дата выдачи", "Последний срок приема", "Дней до просрочки"};
            DataTable table = new DataTable("Outputs");

            foreach (var c in col)
                table.Columns.Add(c);
            
            foreach (var o in outputs)
            {
                var R = new Readers();
                foreach (var r in readers)
                    if (r.r_id == o.R_id)
                        R = r;
                var B = new Books();
                foreach (var b in books)
                    if (b.b_id == o.B_id)
                        B = b;
                table.Rows.Add(
                    o.o_id,
                    R.r_id,
                    R.r_fio,
                    R.r_dt_birth,
                    R.r_passport,
                    B.b_id,
                    B.b_name,
                    B.b_author,
                    B.b_publ,
                    B.b_born,
                    o.o_dt_out,
                    o.o_dt_in,
                    o.o_dt_in - o.o_dt_out
                    );
            }

            ds.Tables.Add(table);

            return View(ds.Tables["Outputs"]);

        }

        public ActionResult Journal()
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            var outputs = db.Outputs;
            return View(outputs.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // GET: Books/Details/5
        public ActionResult DetailsBook(int? id)
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // GET: Books/Create
        public ActionResult CreateBook()
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            var count = db.Books.ToList().Count + 1;
            ViewBag.MIN = count;
            ViewBag.MAX = count;
            return View();
        }

        // POST: Books/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook([Bind(Include = "b_id,b_name,b_author,b_publ,b_born")] Books books)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(books);
                db.SaveChanges();
                return RedirectToAction("Books");
            }

            return View(books);
        }

        // GET: Books/Edit/5
        public ActionResult EditBook(int? id)
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: Books/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook([Bind(Include = "b_id,b_name,b_author,b_publ,b_born")] Books books)
        {
            if (ModelState.IsValid)
            {
                db.Entry(books).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Books");
            }
            return View(books);
        }

        // GET: Books/Delete/5
        public ActionResult DeleteBook(int? id)
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("DeleteBook")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBookConfirmed(int id)
        {
            Books books = db.Books.Find(id);
            db.Books.Remove(books);
            db.SaveChanges();
            return RedirectToAction("Books");
        }

        // GET: Readers/Details/5
        public ActionResult DetailsReader(int? id)
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Readers readers = db.Readers.Find(id);
            if (readers == null)
            {
                return HttpNotFound();
            }
            return View(readers);
        }

        // GET: Readers/Create
        public ActionResult CreateReader()
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            var count = db.Readers.ToList().Count + 1;
            ViewBag.MIN = count;
            ViewBag.MAX = count;
            return View();
        }

        // POST: Readers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReader([Bind(Include = "r_id,r_fio,r_dt_birth,r_passport")] Readers readers)
        {
            if (ModelState.IsValid)
            {
                db.Readers.Add(readers);
                db.SaveChanges();
                return RedirectToAction("Readers");
            }

            return View(readers);
        }

        // GET: Readers/Edit/5
        public ActionResult EditReader(int? id)
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Readers readers = db.Readers.Find(id);
            if (readers == null)
            {
                return HttpNotFound();
            }
            return View(readers);
        }

        // POST: Readers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReader([Bind(Include = "r_id,r_fio,r_dt_birth,r_passport")] Readers readers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(readers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Readers");
            }
            return View(readers);
        }

        // GET: Readers/Delete/5
        public ActionResult DeleteReader(int? id)
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Readers readers = db.Readers.Find(id);
            if (readers == null)
            {
                return HttpNotFound();
            }
            return View(readers);
        }

        // POST: Readers/Delete/5
        [HttpPost, ActionName("DeleteReader")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReaderConfirmed(int id)
        {
            Readers readers = db.Readers.Find(id);
            db.Readers.Remove(readers);
            db.SaveChanges();
            return RedirectToAction("Readers");
        }

        public ActionResult DetailsJournal(int? id)
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outputs outputs = db.Outputs.Find(id);
            if (outputs == null)
            {
                return HttpNotFound();
            }
            Readers readers = db.Readers.Find(outputs.R_id);
            if (readers == null)
            {
                return HttpNotFound();
            }
            Books books = db.Books.Find(outputs.B_id);
            if (books == null)
            {
                return HttpNotFound();
            }
            ViewBag.Readers = readers;
            ViewBag.Books = books;
            return View(outputs);
        }

        // GET: Readers/Create
        public ActionResult CreateJournal()
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            var count = db.Outputs.ToList().Count + 1;
            ViewBag.MIN = count;
            ViewBag.MAX = count;
            ViewData["AllReaders"] = from reader in db.Readers
                                     select new SelectListItem { Text = reader.r_fio, Value = reader.r_id.ToString() };
            ViewData["AllBooks"] = from book in db.Books
                                   select new SelectListItem { Text = book.b_name + "; " + book.b_author, Value = book.b_id.ToString() };
            return View();
        }

        // POST: Readers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJournal([Bind(Include = "o_id, R_id, B_id, o_dt_out, o_dt_in")] Outputs outputs)
        {
            if (ModelState.IsValid)
            {
                db.Outputs.Add(outputs);
                db.SaveChanges();
                return RedirectToAction("Journal");
            }

            return View(outputs);
        }

        // GET: Readers/Edit/5
        public ActionResult EditJournal(int? id)
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outputs outputs = db.Outputs.Find(id);
            if (outputs == null)
            {
                return HttpNotFound();
            }
            Readers readers = db.Readers.Find(outputs.R_id);
            if (readers == null)
            {
                return HttpNotFound();
            }
            Books books = db.Books.Find(outputs.B_id);
            if (books == null)
            {
                return HttpNotFound();
            }
            ViewBag.Readers = readers;
            ViewBag.Books = books;
            ViewData["AllReaders"] = from reader in db.Readers
                                    select new SelectListItem { Text = reader.r_fio, Value = reader.r_id.ToString() };
            ViewData["AllBooks"] = from book in db.Books
                                     select new SelectListItem { Text = book.b_name + "; "+ book.b_author, Value = book.b_id.ToString() };
            return View(outputs);
        }

        // POST: Readers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJournal([Bind(Include = "o_id, R_id, B_id, o_dt_out, o_dt_in")] Outputs outputs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outputs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Journal");
            }
            return View(outputs);
        }

        // GET: Readers/Delete/5
        public ActionResult DeleteJournal(int? id)
        {
            ViewBag.user = "Вы не авторизованы";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = "Ваш логин: " + User.Identity.Name;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outputs outputs = db.Outputs.Find(id);
            if (outputs == null)
            {
                return HttpNotFound();
            }
            Readers readers = db.Readers.Find(outputs.R_id);
            if (readers == null)
            {
                return HttpNotFound();
            }
            Books books = db.Books.Find(outputs.B_id);
            if (books == null)
            {
                return HttpNotFound();
            }
            ViewBag.Readers = readers;
            ViewBag.Books = books;
            return View(outputs);
        }

        // POST: Readers/Delete/5
        [HttpPost, ActionName("DeleteJournal")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteJournalConfirmed(int id)
        {
            Outputs outputs = db.Outputs.Find(id);
            db.Outputs.Remove(outputs);
            db.SaveChanges();
            return RedirectToAction("Journal");
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
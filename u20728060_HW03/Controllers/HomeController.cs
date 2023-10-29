using u20728060_HW03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using PagedList;

namespace u20728060_HW03.Controllers
{
    public class HomeController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        public async Task<ActionResult> CombinedHIndex(int? studentsPage, int? booksPage)
        {
            int studentsPageNr = studentsPage ?? 1;
            int booksPageNr = booksPage ?? 1;
            int pageSize = 10;

            var students = await db.students.ToListAsync();
            var books = await db.books.Include(b => b.borrows).ToListAsync();
            var bookViewModels = books.Select(book => new BookStatus
            {
                Book = book,
                Status = GetBookStatus(book)
            }).ToList();

            var viewModel = new HomeViewModel
            {
                students = students.ToPagedList(studentsPageNr, pageSize),
                books = books.ToPagedList(booksPageNr, pageSize)
            };
            return View(viewModel);
        }

        private string GetBookStatus(book book)
        {
            var today = DateTime.Today;
            var borrow = book.borrows.FirstOrDefault(b =>
                b.broughtDate == null && b.takenDate <= today);

            return borrow != null ? "Out" : "Available";
        }

        public async Task<ActionResult> StudentIndex()
        {
            
            return View(await db.students.ToListAsync());
        }

        public ActionResult StudentCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentCreate([Bind(Include = "studentId, name, surname, birthdate, gender, class, point")] student student)
        {
            
            if (ModelState.IsValid)
            {
                db.students.Add(student);
                await db.SaveChangesAsync();
                return RedirectToAction("StudentIndex");
            }

            return View(student);
        }

        public async Task<ActionResult> BookIndex()
        {
            var book = db.books.Include(b => b.borrows);
            return View(await book.ToListAsync());
        }

        public ActionResult bookCreate()
        {
            ViewBag.BorrowID = new SelectList(db.borrows, "borrowId", "studentId", "bookId", "takenDate", "broughtDate");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> bookCreate([Bind(Include = "bookId,name ,pagecount ,point,authorId,typeId")] book books)
        {
            if (ModelState.IsValid)
            {
                db.books.Add(books);
                await db.SaveChangesAsync();
                return RedirectToAction("BookIndex");
            }

            ViewBag.BorrowID = new SelectList(db.borrows, "borrowId", "studentId", "bookId", "takenDate", "broughtDate");
            return View(books);
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}
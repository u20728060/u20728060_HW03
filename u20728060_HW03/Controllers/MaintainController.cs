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
using System.Web.UI.WebControls;

namespace u20728060_HW03.Controllers
{
    public class MaintainController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        // GET: Maintain
        public async Task<ActionResult> CombinedMIndex(int? authorsPage, int? typesPage, int? borrowsPage )
        {
            int authorsPageNr = authorsPage ?? 1;
            int typesPageNr = typesPage ?? 1;
            int borrowsPageNr = borrowsPage ?? 1;
            int pageSize = 10;

            var authors = await db.authors.ToListAsync();
            var types = await db.types.ToListAsync();
            var borrows = await db.borrows.ToListAsync();
            var viewModel = new MaintainViewModel
            {
                authors = authors.ToPagedList(authorsPageNr, pageSize),
                types = types.ToPagedList(typesPageNr, pageSize),
                borrows = borrows.ToPagedList(borrowsPageNr, pageSize)
            };
            return View(viewModel);
        }

        public async Task<ActionResult> AuthorIndex()
        {
            return View(await db.authors.ToListAsync());
        }

        public ActionResult AuthorCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthorCreate([Bind(Include = "authorId,name,surname")] author authors)
        {
            if (ModelState.IsValid)
            {
                db.authors.Add(authors);
                await db.SaveChangesAsync();
                return RedirectToAction("AuthorIndex");
            }

            return View(authors);
        }

        public async Task<ActionResult> AuthorEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            author authors = await db.authors.FindAsync(id);
            if (authors == null)
            {
                return HttpNotFound();
            }

            return View(authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthorEdit([Bind(Include = "authorId,name,surname")] author authors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(authors).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("AuthorIndex");
            }
            
            return View(authors);
        }

        public async Task<ActionResult> AuthorDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            author authors = await db.authors.FindAsync(id);
            if (authors == null)
            {
                return HttpNotFound();
            }
            return View(authors);
        }

        [HttpPost, ActionName("AuthorDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthorDeleteConfirmed(int id)
        {
            author authors = await db.authors.FindAsync(id);
            db.authors.Remove(authors);
            await db.SaveChangesAsync();
            return RedirectToAction("AuthorIndex");
        }

        public async Task<ActionResult> TypesIndex()
        {
            return View(await db.types.ToListAsync());
        }

        public ActionResult TypesCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TypesCreate([Bind(Include = "typeId,name")] type types)
        {
            if (ModelState.IsValid)
            {
                db.types.Add(types);
                await db.SaveChangesAsync();
                return RedirectToAction("TypesIndex");
            }

            return View(types);
        }

        public async Task<ActionResult> TypesEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            type types = await db.types.FindAsync(id);
            if (types == null)
            {
                return HttpNotFound();
            }
            return View(types);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TypesEdit([Bind(Include = "typeId,name")] type types)
        {
            if (ModelState.IsValid)
            {
                db.Entry(types).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("TypesIndex");
            }
            return View(types);
        }

        public async Task<ActionResult> TypesDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            type types = await db.types.FindAsync(id);
            if (types == null)
            {
                return HttpNotFound();
            }
            return View(types);
        }

        [HttpPost, ActionName("TypesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ListItemDeleteConfirmed(int id)
        {
            type types = await db.types.FindAsync(id);
            db.types.Remove(types);
            await db.SaveChangesAsync();
            return RedirectToAction("TypesIndex");
        }

        public async Task<ActionResult> BorrowsIndex()
        {
            return View(await db.borrows.ToListAsync());
        }

        public ActionResult BorrowsCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BorrowsCreate([Bind(Include = "borrowId,studentId,bookId,takenDate,broughtDate")] borrow borrows)
        {
            if (ModelState.IsValid)
            {
                db.borrows.Add(borrows);
                await db.SaveChangesAsync();
                return RedirectToAction("BorrowsIndex");
            }

            return View(borrows);
        }

        public async Task<ActionResult> BorrowsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            borrow borrows = await db.borrows.FindAsync(id);
            if (borrows == null)
            {
                return HttpNotFound();
            }
            return View(borrows);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BorrowsEdit([Bind(Include = "borrowId,studentId,bookId,takenDate,broughtDate")] borrow borrows)
        {
            if (ModelState.IsValid)
            {
                db.Entry(borrows).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("BorrowsIndex");
            }
            return View(borrows);
        }

        public async Task<ActionResult> BorrowsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            borrow borrows = await db.borrows.FindAsync(id);
            if (borrows == null)
            {
                return HttpNotFound();
            }
            return View(borrows);
        }

        [HttpPost, ActionName("BorrowsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BorrowsDeleteConfirmed(int id)
        {
            borrow borrows = await db.borrows.FindAsync(id);
            db.borrows.Remove(borrows);
            await db.SaveChangesAsync();
            return RedirectToAction("BorrowsIndex");
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
    }
}
using u20728060_HW03.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using OfficeOpenXml;

namespace u20728060_HW03.Controllers
{
    public class ReportController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        // GET: Report
        public ActionResult Report()
        {
            var novemberBorrows = db.borrows
                 .Where(borrow => borrow.takenDate.HasValue && borrow.takenDate.Value.Month == 11)
                      .Join(db.books,
                      borrow => borrow.bookId,
                      book => book.bookId,
                      (borrow, book) => new
                      {
                          Label = book.name,
                          Y = 1,
                          LegendText = book.name
                      })
                .GroupBy(joined => joined.Label)
                .Select(group => new ChartViewModel
                {
                    Label = group.Key,
                    Y = group.Sum(item => item.Y),
                    LegendText = group.Key,

                })
                 .Where(result => result.Y >= 10)
                 .ToList();

            //ViewBag.ChartData = novemberBorrows;

            var tableData = db.borrows
       .Where(borrow => borrow.takenDate.HasValue && borrow.takenDate.Value.Month == 11)
       .Join(db.books, borrow => borrow.bookId, book => book.bookId, (borrow, book) => new
       {
           BookName = book.name,
           BookId = book.bookId,

       })
       .GroupBy(joined => new { joined.BookName, joined.BookId })
       .Select(group => new TableViewModel
       {
           BookName = group.Key.BookName,
           TimesBorrowed = group.Count(),

       })
       .Where(result => result.TimesBorrowed >= 10)
       .ToList();

            var viewModel = new ReportViewModel
            {
                ChartData = novemberBorrows,
                TableData = tableData
            };

            ViewBag.ReportData = viewModel;

            return View();


        }

        public ActionResult downloads(string file)
        {
            string filepath = Server.MapPath("~/pdfs/" + file);

            if(System.IO.File.Exists(filepath))
            {
                byte[] filebytes = System.IO.File.ReadAllBytes(filepath);
                return File(filebytes, "application/pdf", file);
            }
            else
            {
                return HttpNotFound("File not Found");
            }
        }

        [HttpPost]
        public ActionResult ExportTableData(string fileName)
        {
            var tableData = db.borrows
                .Where(borrow => borrow.takenDate.HasValue && borrow.takenDate.Value.Month == 11)
                .Join(db.books, borrow => borrow.bookId, book => book.bookId, (borrow, book) => new
                {
                    BookName = book.name,
                    BookId = book.bookId,
                })
                .GroupBy(joined => new { joined.BookName, joined.BookId })
                .Select(group => new TableViewModel
                {
                    BookName = group.Key.BookName,
                    TimesBorrowed = group.Count(),
                })
                .Where(result => result.TimesBorrowed >= 10)
                .ToList();

            if (string.IsNullOrEmpty(fileName))
            {
                // If the fileName is not provided, you can set a default or handle the error as needed.
                fileName = "DefaultFileName";
            }

            var fileDownloadName = fileName + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:tt") + ".xlsx";

            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("TableData");
            worksheet.Cells[1, 1].LoadFromCollection(tableData, true);

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", $"attachment; filename={fileDownloadName}");
                excelPackage.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

            return View(); // Redirect to the same view or another action as needed.
        }


    }
    }

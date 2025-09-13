using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using updateddetyraep.Models;

namespace updateddetyraep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ContactsBookDBContext _context;

        public HomeController(ILogger<HomeController> logger, ContactsBookDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contacts(string searchString)
        {
            var contacts = _context.Contacts.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(c => c.FirstName.Contains(searchString));
            }

            return View(contacts.ToList());
        }

        public IActionResult ExportToExcel()
        {
            var contacts = _context.Contacts.ToList();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Contacts");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "First Name";
            worksheet.Cell(1, 3).Value = "Last Name";
            worksheet.Cell(1, 4).Value = "Email";
            worksheet.Cell(1, 5).Value = "Phone Number";
            worksheet.Cell(1, 6).Value = "Second Phone Number";
            worksheet.Cell(1, 7).Value = "Address";

            int row = 2;
            foreach (var c in contacts)
            {
                worksheet.Cell(row, 1).Value = c.Id;
                worksheet.Cell(row, 2).Value = c.FirstName;
                worksheet.Cell(row, 3).Value = c.LastName;
                worksheet.Cell(row, 4).Value = c.Email;
                worksheet.Cell(row, 5).Value = c.PhoneNumber;
                worksheet.Cell(row, 6).Value = c.PhoneNumber2;
                worksheet.Cell(row, 7).Value = c.Adress ?? "-";
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contacts.xlsx");
        }

        public IActionResult CreateEditContacts(int? id)
        {
            if (id.HasValue)
            {
                var contact = _context.Contacts.Find(id.Value);
                if (contact == null) return NotFound();
                return View(contact);
            }

            return View(new Contact());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditContacts(Contact model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Id == 0)
                _context.Contacts.Add(model);
            else
                _context.Contacts.Update(model);

            _context.SaveChanges();
            return RedirectToAction("Contacts");
        }

        public IActionResult DeleteContacts(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
            }
            return RedirectToAction("Contacts");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

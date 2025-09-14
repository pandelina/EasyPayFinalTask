using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using updateddetyraep.Models;
using System.Collections.Generic;

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
            var contacts = _context.Contacts
                                   .Include(c => c.PhoneNumbers)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                contacts = contacts.Where(c => c.FirstName.Contains(searchString));

            return View(contacts.ToList());
        }

        public IActionResult ExportToExcel()
        {
            var contacts = _context.Contacts
                                   .Include(c => c.PhoneNumbers)
                                   .ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Contacts");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "First Name";
            worksheet.Cell(1, 3).Value = "Last Name";
            worksheet.Cell(1, 4).Value = "Email";
            worksheet.Cell(1, 5).Value = "Phone Numbers";
            worksheet.Cell(1, 6).Value = "Address";

            int row = 2;
            foreach (var contact in contacts)
            {
                worksheet.Cell(row, 1).Value = contact.Id;
                worksheet.Cell(row, 2).Value = contact.FirstName;
                worksheet.Cell(row, 3).Value = contact.LastName;
                worksheet.Cell(row, 4).Value = contact.Email;
                worksheet.Cell(row, 5).Value = string.Join(", ", contact.PhoneNumbers.Select(p => p.Number));
                worksheet.Cell(row, 6).Value = contact.Adress ?? "-";
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contacts.xlsx");
        }

        public IActionResult CreateEditContacts(int? id)
        {
            if (id.HasValue)
            {
                var contact = _context.Contacts
                                      .Include(c => c.PhoneNumbers)
                                      .FirstOrDefault(c => c.Id == id.Value);
                if (contact == null) return NotFound();
                return View(contact);
            }

            return View(new Contact());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditContacts(Contact model, List<string> phoneNumbers)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Id == 0)
            {
                _context.Contacts.Add(model);
                _context.SaveChanges(); // ruaj për të marrë Id

                foreach (var number in phoneNumbers.Where(n => !string.IsNullOrWhiteSpace(n)))
                {
                    _context.PhoneNumbers.Add(new PhoneNumber
                    {
                        Number = number,
                        ContactId = model.Id
                    });
                }
            }
            else
            {
                var existing = _context.Contacts
                                       .Include(c => c.PhoneNumbers)
                                       .FirstOrDefault(c => c.Id == model.Id);

                if (existing != null)
                {
                    existing.FirstName = model.FirstName;
                    existing.LastName = model.LastName;
                    existing.Email = model.Email;
                    existing.Adress = model.Adress;

                    _context.PhoneNumbers.RemoveRange(existing.PhoneNumbers);

                    foreach (var number in phoneNumbers.Where(n => !string.IsNullOrWhiteSpace(n)))
                    {
                        _context.PhoneNumbers.Add(new PhoneNumber
                        {
                            Number = number,
                            ContactId = existing.Id
                        });
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Contacts");
        }

        public IActionResult DeleteContacts(int id)
        {
            var contact = _context.Contacts
                                  .Include(c => c.PhoneNumbers)
                                  .FirstOrDefault(c => c.Id == id);

            if (contact != null)
            {
                _context.PhoneNumbers.RemoveRange(contact.PhoneNumbers);
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
            }

            return RedirectToAction("Contacts");
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}

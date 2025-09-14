using Microsoft.AspNetCore.Mvc;
using updateddetyraep.Models;

namespace updateddetyraep.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsApiController : ControllerBase
    {
        private readonly ContactsBookDBContext _context;

        public ContactsApiController(ContactsBookDBContext context)
        {
            _context = context;
        }

        // GET: api/contactsapi
        [HttpGet]
        public IActionResult GetContacts()
        {
            var contacts = _context.Contacts.ToList();
            return Ok(contacts);
        }

        // GET: api/contactsapi/5
        [HttpGet("{id}")]
        public IActionResult GetContact(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
                return NotFound();

            return Ok(contact);
        }

        // POST: api/contactsapi
        [HttpPost]
        public IActionResult CreateContact([FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        // DELETE: api/contactsapi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
                return NotFound();

            _context.Contacts.Remove(contact);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

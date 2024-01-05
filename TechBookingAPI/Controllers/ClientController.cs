using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechBookingAPI.Models.ORM;

namespace TechBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly TechDbContext _context;

        public ClientController(TechDbContext context)
        {
            _context = context;
        }

        // GET: api/Client
        [HttpGet]
        public IActionResult GetClients()
        {
            var clients = _context.Clients.ToList();
            return Ok(clients);
           
        }
        // GET: api/Client/5
        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public IActionResult PostClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //DELETE: api/Client/5
        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            _context.Clients.Remove(client);
            _context.SaveChanges();
            return Ok(client);
        }

        // PUT: api/Client/5
        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, [FromBody] Client updatedClient)
        {
            if (id != updatedClient.Id)
            {
                return BadRequest("Client id mismatch");
            }

            // İlgili müşteriyi veritabanından bul
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound("Client not found");
            }

            // Müşteri bilgilerini güncelle
            client.Name = updatedClient.Name;
            client.Surname = updatedClient.Surname;
            client.Email = updatedClient.Email;
            client.BirthDate = updatedClient.BirthDate;
            client.Address = updatedClient.Address;
            client.CompanyId = updatedClient.CompanyId;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound("Client not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}

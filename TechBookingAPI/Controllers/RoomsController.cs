using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechBookingAPI.Models.ORM;

namespace TechBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly TechDbContext _context;

        public RoomsController(TechDbContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        public IActionResult GetRooms()
        {
            var rooms = _context.Rooms.ToList();
            return Ok(rooms);
        }
        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public IActionResult GetRoom(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        public IActionResult PostRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return Ok(room);
        }
        // PUT: api/Rooms/5
        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            if (id != updatedRoom.Id)
            {
                return BadRequest("Room id mismatch");
            }

            // İlgili odayı veritabanından bul
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            room.Name = updatedRoom.Name;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomsExists(id))
                {
                    return NotFound("Room not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool RoomsExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }

    }
}

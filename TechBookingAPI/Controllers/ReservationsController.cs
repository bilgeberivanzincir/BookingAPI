using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechBookingAPI.Models.ORM;

namespace TechBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly TechDbContext _context;

        public ReservationsController(TechDbContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        public IActionResult GetReservations()
        {
            var reservations = _context.Reservations.Include(r => r.Room).Include(r => r.Client.Company).ToList();
            return Ok(reservations);
           
        }
        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public IActionResult GetReservation(int id)
        {
            var reservation = _context.Reservations.Include(r => r.Room).Include(r => r.Client.Company).FirstOrDefault(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult PostReservation(Reservation reservation)
        { 
            if(reservation==null)
            {
                return BadRequest("Reservation is null");
            }
            if(reservation.ClientId == 0 || reservation.RoomId == 0)
            {
                return BadRequest("Client or Room id is missing");
            }
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);


        }

        //DELETE: api/Reservations/1
        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            return Ok(reservation);
        }

        // PUT: api/Reservations/1
        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
        {
            if (id != updatedReservation.Id)
            {
                return BadRequest("Reservation id mismatch");
            }

            // İlgili rezervasyonu veritabanından bul
            var reservation = _context.Reservations.Find(id);

            if (reservation == null)
            {
                return NotFound("Reservation not found");
            }
     
            reservation.ReservationDuration = updatedReservation.ReservationDuration;
            reservation.ReservationTime = updatedReservation.ReservationTime;
            reservation.ClientId = updatedReservation.ClientId;
            reservation.RoomId = updatedReservation.RoomId;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RezervationExists(id))
                {
                    return NotFound("Reservation not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool RezervationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}

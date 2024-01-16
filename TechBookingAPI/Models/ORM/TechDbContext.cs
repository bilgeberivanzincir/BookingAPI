using Microsoft.EntityFrameworkCore;

namespace TechBookingAPI.Models.ORM
{
    public class TechDbContext : DbContext
    {
   
        public TechDbContext(DbContextOptions<TechDbContext> options) : base(options)
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
    
    
}

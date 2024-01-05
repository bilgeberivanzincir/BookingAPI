namespace TechBookingAPI.Models.ORM
{
    public class Reservation : BaseModel
    {
        public DateTime ReservationTime { get; set; }
        public int ReservationDuration { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } // one to many relation
        public int RoomId { get; set; }
        public Room Room { get; set; } // one to many relation

    }
}

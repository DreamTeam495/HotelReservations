using System;

namespace HotelReservationAPI.Model
{
    public class Reservation
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long RoomId { get; set; }
        public int PersonCount { get; set; }
        public string? ClientName { get; set; }
        public string? ClientEmail { get; set; }
    }
}

namespace HotelReservation.Areas.Rooms.Models;

public class Reservation
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    
    public Customer Customer { get; set; }
    public Room Room { get; set; }
}
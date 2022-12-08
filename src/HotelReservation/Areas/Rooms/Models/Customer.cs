namespace HotelReservation.Areas.Rooms.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string CreditCardNumber { get; set; }
    public string CreditCardExpiration { get; set; }
    public string CreditCardCvv { get; set; }
}
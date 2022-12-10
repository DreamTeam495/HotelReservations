using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using HotelReservation.Utilities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace HotelReservation.Areas.Rooms.Pages;

public class Confirmation : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public Confirmation(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public Rooms.Models.Reservation? Reserve { get; set; }

    [BindProperty]
    public int NumberOfDays { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Reserve = await _dbContext.Reservations
            .Include(x => x.Customer)
            .Include(x => x.Room)
            .SingleAsync(y => y.Id == HttpContext.Session.Get<int>("_ReservationID"));

        NumberOfDays = (Reserve.EndDate - Reserve.StartDate).Days;
        
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress ("DreamHotel495", "reservation@dreamhotel.com"));
        message.To.Add (new MailboxAddress ($"{Reserve.Customer.FirstName} {Reserve.Customer.LastName}", $"{Reserve.Customer.Email}"));
        message.Subject = $"Your reservation at Dream Hotel, {Reserve.Customer.FirstName}!";
        message.Body = new TextPart ("plain") {
            Text = @$"{Reserve.Customer.FirstName},
Thank you for reservation at Dream Hotel, we hope you'll enjoy your stay.

Reservation information: 
Your reservation id is: {Reserve.Id}
The room you've reserved is: {Reserve.Room.Description}
Check-in: {Reserve.StartDate} at 4:00 PM.
Check-out: {Reserve.EndDate} at 10:00 AM.

You've paid: {Reserve.Price.ToString("C")}

If you need to cancel your reservation you will need to enter your reservation id and email.
id: {Reserve.Id}
email: {Reserve.Customer.Email}

If you have any questions or issues please do not hesitate to reach out to reservation@dreamhotel.com

Thanks again for your patronage!
- DreamHotel495
"
        };

        using (var client = new SmtpClient ()) {
            client.Connect ("localhost", 25, false);
            
            client.Send (message);
            client.Disconnect (true);
        }
        
        HttpContext.Session.Clear();
        
        return Page();
    }
}
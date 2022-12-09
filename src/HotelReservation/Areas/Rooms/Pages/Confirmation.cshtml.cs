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
        
        //TODO: Formatting funky
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress ("DreamHotel495", "reservation@dreamhotel.com"));
        message.To.Add (new MailboxAddress ($"{Reserve.Customer.FirstName} {Reserve.Customer.LastName}", $"{Reserve.Customer.Email}"));
        message.Subject = "Thank you for reserving!";
        message.Body = new TextPart ("plain") {
            Text = @$"{Reserve.Customer.FirstName},
Your reservation id is: {Reserve.Id}
Starting from {Reserve.StartDate.ToShortDateString()} to {Reserve.EndDate.ToShortDateString()} of the {Reserve.Room.Description} type.
Price: {Reserve.Price.ToString("C")}
Has been made
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
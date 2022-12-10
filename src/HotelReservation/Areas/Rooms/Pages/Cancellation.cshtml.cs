using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using HotelReservation.Utilities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace HotelReservation.Areas.Rooms.Pages;

public class Cancellation : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public Cancellation(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public Rooms.Models.Reservation? Reserve { get; set; }

    [BindProperty]
    public string Email { get; set; }
    
    [BindProperty]
    public int ConfirmationNumber { get; set; }
    
    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostOnSubmitAsync()
    {
        Reserve = await _dbContext.Reservations
            .Include(x => x.Customer)
            .Include(x => x.Room)
            .Where(x => x.Customer.Email == Email)
            .Where(x => x.Id == ConfirmationNumber)
            .SingleAsync();

        _dbContext.Reservations.Remove(Reserve);
        
        await _dbContext.SaveChangesAsync();
        
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress ("DreamHotel495", "reservation@dreamhotel.com"));
        message.To.Add (new MailboxAddress ($"{Reserve.Customer.FirstName} {Reserve.Customer.LastName}", $"{Reserve.Customer.Email}"));
        message.Subject = $"We're sorry to see you go, {Reserve.Customer.FirstName}!";
        message.Body = new TextPart ("plain") {
            Text = @$"{Reserve.Customer.FirstName},
You've successfully cancelled your reservation at Dream Hotel starting from {Reserve.StartDate.ToShortDateString()} to {Reserve.EndDate.ToShortDateString()}.

In the next 3-5 business you should see {Reserve.Price.ToString("C")} added back to your account.

We apologize for any inconveniences this refund may cause you.

Feel free to contact our team at reservation@dreamhotel.com should you have any questions.

- DreamHotel495
"
        };

        using (var client = new SmtpClient ()) {
            client.Connect ("localhost", 25, false);
            
            client.Send (message);
            client.Disconnect (true);
        }

        return RedirectToPage("Index");
    }
    
    public async Task<IActionResult> OnPostOnCancelAsync()
    {
        return RedirectToPage("Index");
    }
}
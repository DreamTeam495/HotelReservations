using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using HotelReservation.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Areas.Rooms.Pages;

//TODO: SMTP4Dev
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
            .SingleAsync(x => x.Id == HttpContext.Session.Get<int>("_ReservationID"));
        
        NumberOfDays = (Reserve.EndDate - Reserve.StartDate).Days;
        
        return Page();
    }
}
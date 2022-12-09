using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using HotelReservation.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Areas.Rooms.Pages;

public class Reservation : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public Reservation(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Room> Rooms { get; set; }

    [BindProperty]
    public Room? Input { get; set; }

    [BindProperty]
    public DateTime StartDate { get; set; }
    
    [BindProperty]
    public DateTime EndDate { get; set; }

    public const string SDate = "_StartDate";
    public const string EDate = "_EndDate";

    //TODO: Only return available rooms for given date range
    public async Task<IActionResult> OnGetAsync([FromQuery]DateTime startDate, [FromQuery]DateTime endDate)
    {
        if (HttpContext.Session.Get<DateTime>(SDate) == default && HttpContext.Session.Get<DateTime>(EDate) == default)
        {
            HttpContext.Session.Set(SDate, startDate);
            HttpContext.Session.Set(EDate, endDate);
            StartDate = startDate;
            EndDate = endDate;
        }

        Rooms = await _dbContext.Rooms.ToListAsync();
        return Page();
    }

    //TODO: Date passes through as a TempData, should I set this in DateSelect rather than reservation?
    public async Task<IActionResult> OnPostOnReserveAsync()
    {
        var room = await _dbContext.Rooms.SingleAsync(x => x.Id == Input.Id);

        await _dbContext.SaveChangesAsync();

        return RedirectToPage("CustomerInformation", new { Input.Id });
    }
}
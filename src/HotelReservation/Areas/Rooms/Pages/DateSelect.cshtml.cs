using System.ComponentModel.DataAnnotations;
using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using HotelReservation.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Areas.Rooms.Pages;

public class DateSelect : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public DateSelect(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Room> Rooms { get; set; }
    
    [BindProperty]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today;
    
    [BindProperty]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);

    public const string SDate = "_StartDate";
    public const string EDate = "_EndDate";

    public async Task<IActionResult> OnGetAsync()
    {
        Rooms = await _dbContext.Rooms.ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.Get<DateTime>(SDate) == default && HttpContext.Session.Get<DateTime>(EDate) == default)
        {
            HttpContext.Session.Set(SDate, StartDate);
            HttpContext.Session.Set(EDate, EndDate);
        }
        
        return RedirectToPage("RoomSelect");
    }
}
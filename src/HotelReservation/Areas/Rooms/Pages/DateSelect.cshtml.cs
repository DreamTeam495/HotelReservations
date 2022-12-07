using System.ComponentModel.DataAnnotations;
using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Areas.Rooms.Pages;

public class DateSelect : PageModel
{
    [BindProperty]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [BindProperty]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    private readonly ApplicationDbContext _dbContext;

    public DateSelect(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Room> Rooms { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Rooms = await _dbContext.Rooms.ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        return RedirectToPage("Reservation", new {StartDate = StartDate, EndDate = EndDate});
    }
}
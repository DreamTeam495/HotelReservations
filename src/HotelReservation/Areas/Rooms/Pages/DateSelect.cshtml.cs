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
        return Page();
    }

    /// <summary>
    /// On dates searched, provided check-in and check-out dates are saved into a session.
    /// </summary>
    /// <returns>Swaps view to next page.</returns>
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
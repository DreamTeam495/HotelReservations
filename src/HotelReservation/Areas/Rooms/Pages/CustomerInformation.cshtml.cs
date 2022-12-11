using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using HotelReservation.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Areas.Rooms.Pages;

// TODO: Create data validation for user email / credit card
public class CustomerInformation : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerInformation(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [BindProperty]
    public Customer? Input { get; set; }
    
    [BindProperty]
    public Room? Output { get; set; }
    
    [BindProperty]
    public Reservation? Reserve { get; set; }
    
    [BindProperty]
    public decimal Price { get; set; }
    
    public DateTime StartDate { get; set; }
    
    [BindProperty]
    public DateTime EndDate { get; set; }

    public const string TotalCost = "_TotalCost";
    public const string RoomId = "_RoomID";
    public const string ReservationId = "_ReservationID";

    public async Task<IActionResult> OnGetAsync([FromQuery] int id)
    {
        Output = await _dbContext.Rooms.SingleAsync(x => x.Id == id);
        StartDate = HttpContext.Session.Get<DateTime>("_StartDate");
        EndDate = HttpContext.Session.Get<DateTime>("_EndDate");
        Price = (EndDate - StartDate).Days * Output.Price;

        HttpContext.Session.Set(RoomId, id);
        HttpContext.Session.Set(TotalCost, Price);
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostOnSubmitAsync()
    {
        _dbContext.Customers.Add(Input);
        
        await _dbContext.SaveChangesAsync();
        
        Output = await _dbContext.Rooms.SingleAsync(x => x.Id == HttpContext.Session.Get<int>("_RoomID"));
        StartDate = HttpContext.Session.Get<DateTime>("_StartDate");
        EndDate = HttpContext.Session.Get<DateTime>("_EndDate");
        Price = HttpContext.Session.Get<decimal>("_TotalCost");

        var reservation = new Reservation
        {
            StartDate = StartDate,
            EndDate = EndDate,
            Customer = Input,
            Price = Price,
            Room = Output
        };

        _dbContext.Reservations.Add(reservation);
        
        await _dbContext.SaveChangesAsync();
        
        HttpContext.Session.Set(ReservationId, reservation.Id);
        
        return RedirectToPage("Confirmation");
    }
    
    public async Task<IActionResult> OnPostOnCancelAsync()
    {
        return RedirectToPage("RoomSelect");
    }
}
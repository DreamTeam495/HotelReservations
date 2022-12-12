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

    /// <summary>
    /// On page load, room type is stored from query parameter.
    /// StartDate and EndDate is also retrieved from session.
    /// Price is calculated in code behind to ensure stateless pages.
    /// As user is inputting customer information it is assumed that this room is the final choice and Id is stored in session.
    /// Price is also stored in session to be passed down to database input on post.
    /// </summary>
    /// <param name="id">Room Id from previous page</param>
    /// <returns></returns>
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
    
    /// <summary>
    /// All customer information from user input is stored in customers database.
    /// Room is found based on session Id.
    /// StartDate and EndDate are retrieved from session.
    /// Reservation is created based on user input and session variables and stored in reservation database.
    /// Reservation.Id is stored in session to passed along to confirmation email
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// Deletes stored StartDate, EndDate, roomId, and Price in session ensure correct data is used.
    /// If user selects cancel, user is returned index to begin process over again
    /// </summary>
    public async Task<IActionResult> OnPostOnCancelAsync()
    {
        HttpContext.Session.Remove("_StartDate");
        HttpContext.Session.Remove("_EndDate");
        HttpContext.Session.Remove("_RoomId");
        HttpContext.Session.Remove("_TotalCost");
        
        return RedirectToPage("Index");
    }
}
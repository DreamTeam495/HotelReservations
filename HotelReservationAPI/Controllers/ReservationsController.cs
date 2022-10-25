using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelReservationAPI.Contexts;
using HotelReservationAPI.Model;

namespace HotelReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationContext _context;

        public ReservationsController(ReservationContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationItems()
        {
            return await _context.ReservationItems.ToListAsync();
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(long id)
        {
            var reservation = await _context.ReservationItems.FindAsync(id);
            if (reservation == null)
                return NotFound();
            return reservation;
        }

        [HttpGet]
        [Route("search/{start}/{end}/{roomId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations(DateTime ?start, DateTime? end, long? roomId = null)
        {
            List<Reservation> returnV = await _context.ReservationItems.Where(t => t.Date > start && t.Date < end && (roomId == null || t.RoomId == roomId)).ToListAsync();
            if (returnV == null || returnV.Count == 0)
                return NotFound();
            return returnV;
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(long id, Reservation reservation)
        {
            if (id != reservation.Id)
                return BadRequest();

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            _context.ReservationItems.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction( nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(long id)
        {
            var reservation = await _context.ReservationItems.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.ReservationItems.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("latestid")]
        public async Task<ActionResult<long>> GetLatestId()
        {
            Reservation res = await _context.ReservationItems.OrderByDescending(a => a.Id).FirstAsync();
            if (res == null)
                return NotFound();
            return res.Id;
        }

        private bool ReservationExists(long id)
        {
            return _context.ReservationItems.Any(e => e.Id == id);
        }
    }
}

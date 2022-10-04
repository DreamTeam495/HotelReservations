using HotelReservationAPI;
using HotelReservationAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservations.Model.Providers
{
    /// <summary>
    /// Provides access to a database of reservations.
    /// </summary>
    interface IReservationProvider
    {
        /// <summary>
        /// Searches for a reservation by id.
        /// </summary>
        /// <param name="reservationId">The id of the reservation to search for. </param>
        /// <returns>The reservation, or null if not found.</returns>
        public Task<Reservation> QueryReservation(long reservationId);

        /// <summary>
        /// Searches for reservations that are between the start and end time and have the room id listed.
        /// </summary>
        /// <param name="start">Start of the time frame to search in. null means there is no start time.</param>
        /// <param name="end">End of the time frame to search in. null means there is no end time.</param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public Task<IEnumerable<Reservation>> QueryReservations(DateTime? start = null, DateTime? end = null, long roomId = -1);

        /// <summary>
        /// Adds a reservation to the database. Fails if there already is a database entry with the same id. If given a negative id, a new id will automatically be assigned.
        /// </summary>
        /// <param name="reservation">The reservation to add.</param>
        /// <returns>True on success, false on failure.</returns>
        public Task<bool> AddReservation(Reservation reservation);

        /// <summary>
        /// Removes a reservation.
        /// </summary>
        /// <param name="reservation">The id of the reservation to remove.</param>
        /// <returns>True on success, false on failure.</returns>
        public Task<bool> RemoveReservation(long reservation);

        /// <summary>
        /// Updates an already existing reservation.
        /// </summary>
        /// <param name="reservation">The updated version of the reservation.</param>
        /// <returns>The updated reservation, or null on failure. </returns>
        public Task<Reservation> UpdateReservation(Reservation reservation);
    }
}

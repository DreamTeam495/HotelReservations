using HotelReservationAPI;
using HotelReservationAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservations.Model.Providers
{
    interface IReservationProvider
    {
        public Task<bool> Initialize();
        public Task<Reservation> QueryReservation(long reservationId);
        public Task<IEnumerable<Reservation>> QueryReservations(DateTime? start = null, DateTime? end = null, long roomId = -1);
        public Task<bool> AddReservation(Reservation reservation);
        public Task<bool> RemoveReservation(long reservation);
        public Task<Reservation> UpdateReservation(Reservation reservation);
    }
}

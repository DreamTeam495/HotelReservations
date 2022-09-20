using HotelReservationAPI.Model;
using HotelReservations.Model.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;

namespace HotelReservations.Model
{
    internal class RestReservationProvider : IReservationProvider
    {
        RestReservationProvider()
        {
            _client = new HttpClient { BaseAddress = new Uri(_tmpAddress) };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> AddReservation(Reservation reservation)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/reservations", reservation);
            return response.IsSuccessStatusCode;
        }

        public async Task<Reservation> QueryReservation(long reservationId)
        {
            Reservation reservation = null;
            HttpResponseMessage response = await _client.GetAsync($"api/reservations{reservationId}");
            if (response.IsSuccessStatusCode)
                reservation = await response.Content.ReadFromJsonAsync<Reservation>();
            return reservation;
        }

        public async Task<IEnumerable<Reservation>> QueryReservations(DateTime? start = null, DateTime? end = null, long roomId = -1)
        {
            start = start ?? new DateTime();
            end = end ?? DateTime.MaxValue;

            IEnumerable<Reservation> reservation = null;
            HttpResponseMessage response = await _client.GetAsync($"api/reservations/search{start}/{end}/{roomId}");
            if (response.IsSuccessStatusCode)
                reservation = await response.Content.ReadFromJsonAsync<IEnumerable<Reservation>>();
            return reservation;
        }

        public async Task<bool> RemoveReservation(long id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/reservations{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Reservation> UpdateReservation(Reservation reservation)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/reservations/{reservation.Id}", reservation);
            if(response.IsSuccessStatusCode)
                reservation = await response.Content.ReadFromJsonAsync<Reservation>();
            return reservation;
        }

        private const string _tmpAddress = "localhost:";
        private const string _responseFormat = "application/json";
        private static HttpClient _client;
    }
}

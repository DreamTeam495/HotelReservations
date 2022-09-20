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

namespace HotelReservations.Model.Providers
{
    class RestRoomProvider : IRoomProvider
    {
        RestRoomProvider()
        {
            _client = new HttpClient { BaseAddress = new Uri(_tmpAddress) };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> AddRoom(Room room)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/rooms", room);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Room>> QueryFree(DateTime start = default, DateTime end = default, int minPeople = -1)
        {
            throw new NotImplementedException();
        }

        public async Task<Room> QueryRoom(long roomId)
        {
            Room room = null;
            HttpResponseMessage response = await _client.GetAsync($"api/reservations{roomId}");
            if (response.IsSuccessStatusCode)
                room = await response.Content.ReadFromJsonAsync<Room>();
            return room;
        }

        public async Task<bool> RemoveRoom(long roomId)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/reservations{roomId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Room> UpdateRoom(Room room)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/reservations/{room.Id}", room);
            if (response.IsSuccessStatusCode)
                room = await response.Content.ReadFromJsonAsync<Room>();
            return room;
        }

        private const string _tmpAddress = "localhost:";
        private const string _responseFormat = "application/json";
        private static HttpClient _client;
    }
}

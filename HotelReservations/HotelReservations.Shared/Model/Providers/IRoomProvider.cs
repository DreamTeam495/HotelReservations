using HotelReservationAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservations.Model.Providers
{
    public interface IRoomProvider
    {
        public Task<bool> Initialize();
        public Task<IEnumerable<Room>> QueryRooms();
        public Task<Room> QueryRoom(long roomId);
        public Task<List<Room>> QueryFree(DateTime? start = null, DateTime? end = null, int minPeople = -1);
        public Task<bool> AddRoom(Room roomId);
        public Task<bool> RemoveRoom(long roomId);
        public Task<Room> UpdateRoom(Room room);
    }
}

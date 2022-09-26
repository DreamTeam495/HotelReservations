using HotelReservationAPI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HotelReservations.Model.Providers
{
    internal class LocalRoomProvider : IRoomProvider
    {
        public LocalRoomProvider()
        {
        }
        
        public async Task<bool> Initialize()
        {
            await Load();
            return true;
        }

        public Task<IEnumerable<Room>> QueryRooms()
        {
            return (Task<IEnumerable<Room>>)(from val in _rooms select val.Value);
        }


        public async Task<bool> AddRoom(Room room)
        {
            _rooms[room.Id] = room;
            await Save();
            return true;
        }

        public async Task<Room> QueryRoom(long roomId)
        {
            if (_rooms.ContainsKey(roomId))
                return _rooms[roomId];
            return null;
        }

        public async Task<List<Room>> QueryFree(DateTime? start = null, DateTime? end = null, int minPeople = -1)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveRoom(long roomId)
        {
            if (!_rooms.ContainsKey(roomId))
                return false;
            _rooms.Remove(roomId);
            await Save();
            return true;
        }

        public async Task<Room> UpdateRoom(Room room)
        {
            if (!_rooms.ContainsKey(room.Id))
                return null;
            _rooms[room.Id] = room;
            await Save();
            return room;
        }

        private async Task Save()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("HotelReservations", CreationCollisionOption.OpenIfExists);
            File.WriteAllText(Path.Combine(folder.Path, "rooms.json"), JsonConvert.SerializeObject(_rooms));
        }

        private async Task Load()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("HotelReservations", CreationCollisionOption.OpenIfExists);

            if (File.Exists(Path.Combine(folder.Path, "rooms.json")))
            {
                string text = await File.ReadAllTextAsync(Path.Combine(folder.Path, "rooms.json"));
                _rooms = JsonConvert.DeserializeObject<Dictionary<long, Room>>(text);
            }
            else
                _rooms = new Dictionary<long, Room>();
        }

        private Dictionary<long, Room> _rooms;
    }
}

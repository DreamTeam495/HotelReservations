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
    internal class LocalReservationProvider : IReservationProvider
    {
        public LocalReservationProvider()
        {
            Task.Run(() => Load()).Wait();
        }

        public async Task<bool> AddReservation(Reservation reservation)
        {
            _reservations[reservation.Id] = reservation;
            await Save();
            return true;
        }

        public async Task<Reservation> QueryReservation(long reservationId)
        {
            if (_reservations.ContainsKey(reservationId))
                return _reservations[reservationId];
            return null;
        }

        public async Task<IEnumerable<Reservation>> QueryReservations(DateTime? start = null, DateTime? end = null, long roomId = -1)
        {
            start ??= DateTime.MinValue;
            end ??= DateTime.MaxValue;
            return from val in _reservations where (val.Value.Date > start && val.Value.Date < end && (val.Value.RoomId == roomId || roomId < 0)) select val.Value;
        }

        public async Task<bool> RemoveReservation(long reservation)
        {
            if (!_reservations.ContainsKey(reservation))
                return false;
            _reservations.Remove(reservation);
            await Save();
            return true;
        }

        public async Task<Reservation> UpdateReservation(Reservation reservation)
        {
            if (!_reservations.ContainsKey(reservation.Id))
                return null;
            _reservations[reservation.Id] = reservation;
            await Save();
            return reservation;
        }


        private async Task Save()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("HotelReservations", CreationCollisionOption.OpenIfExists);
            File.WriteAllText(Path.Combine(folder.Path, "reservations.json"), JsonConvert.SerializeObject(_reservations));
        }

        private async Task Load()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("HotelReservations", CreationCollisionOption.OpenIfExists);

            if (File.Exists(Path.Combine(folder.Path, "reservations.json")))
            {
                string text = await File.ReadAllTextAsync(Path.Combine(folder.Path, "reservations.json"));
                _reservations = JsonConvert.DeserializeObject<Dictionary<long, Reservation>>(text);
            }
            else
                _reservations = new Dictionary<long, Reservation>();
        }

        private Dictionary<long, Reservation> _reservations;
    }
}

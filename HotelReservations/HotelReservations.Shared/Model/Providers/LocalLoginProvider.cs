using HotelReservationAPI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Security.Cryptography;

namespace HotelReservations.Model.Providers
{
    public class LocalLoginProvider : ILoginProvider
    {
        public LocalLoginProvider()
        {
            Task.Run(() => Load()).Wait();
        }

        public async Task<User.EnumUserType> GetUserType()
        {
            if (loggedIn != null)
                return loggedIn.UserType;
            return User.EnumUserType.None;
        }

        public async Task<bool> Logout()
        {
            if(loggedIn != null)
            {
                loggedIn = null;
                return true;
            }
            return false;
        }

        public async Task<User> Login(string email, string password)
        {
            if (await GetUserType() != User.EnumUserType.None)
                Logout();

            List<User> found = _users.Where((value) => value.Value.Email == email).Select((value) => value.Value).ToList();
            if(found != null && found.Count > 0 && SHA512.HashData(Encoding.ASCII.GetBytes(password + found[0].Salt) ) == Encoding.ASCII.GetBytes(found[0].Hash))
            {
                loggedIn = found[0];
                return loggedIn;
            }
            return null;
        }

        public async Task<User> RegisterUser(User user, string password)
        {
            if (await GetUserType() != User.EnumUserType.Admin)
                return null;

            user.Id = GetLatestId() + 1;
            user.Salt = Encoding.ASCII.GetString(GetSalt(32)); ;
            user.Hash = Encoding.ASCII.GetString(SHA512.HashData(Encoding.ASCII.GetBytes(password + user.Salt)));
            _users[user.Id] = user;
            await Save();
            return user;
        }

        public async Task<User> RemoveUser(string email)
        {
            if (await GetUserType() != User.EnumUserType.Admin)
                return null;
            List<User> found = _users.Where((value) => value.Value.Email == email).Select((value) => value.Value).ToList();
            if (found != null && found.Count > 0)
            {
                _users.Remove(found[0].Id);
                await Save();
                return found[0];
            }
            return null;
        }

        private async Task Save()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("HotelReservations", CreationCollisionOption.OpenIfExists);
            File.WriteAllText(Path.Combine(folder.Path, "users.json"), JsonConvert.SerializeObject(_users));
        }

        private async Task Load()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("HotelReservations", CreationCollisionOption.OpenIfExists);

            if (File.Exists(Path.Combine(folder.Path, "users.json")))
            {
                string text = await File.ReadAllTextAsync(Path.Combine(folder.Path, "reservations.json"));
                _users = JsonConvert.DeserializeObject<Dictionary<long, User>>(text);
            }
            else
                _users = new Dictionary<long, User>();
        }

        private long GetLatestId()
        {
            long max = -1;
            foreach (var user in _users)
                if (user.Key > max)
                    max = user.Key;
            return max;
        }

        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private User loggedIn;
        private Dictionary<long, User> _users;
    }
}

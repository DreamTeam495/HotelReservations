using HotelReservationAPI.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Model.Providers
{
    interface ILoginProvider
    {
        /// <summary>
        /// Gets the type of user currently logged in.
        /// </summary>
        /// <returns>The type of user logged in, or none if not logged in.</returns>
        Task<User.EnumUserType> GetUserType();

        /// <summary>
        /// Logs the current active user out.
        /// </summary>
        /// <returns>True if successfull, false otherwise.</returns>
        Task<bool> Logout();

        /// <summary>
        /// Logs a user in with the email and password given.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The plaintext password of the user.</param>
        /// <returns>User data of the logged in user, null if failed.</returns>
        Task<User> Login(string email, string password);

        /// <summary>
        /// Creates a new user if the user is currently logged in.
        /// </summary>
        /// <param name="user">The user data to append.</param>
        /// <param name="password">Password for the user.</param>
        /// <returns>User added, or null on failure.</returns>
        Task<User> RegisterUser(User user, string password);

        /// <summary>
        /// Removes a user.
        /// </summary>
        /// <param name="email">Email of the user to remove.</param>
        /// <returns>User removed, false otherwise.</returns>
        Task<User> RemoveUser(string email);
    }
}

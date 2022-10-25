using HotelReservationAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservations.Model.Providers
{
    /// <summary>
    /// Provides access to a database of rooms.
    /// </summary>
    public interface IRoomProvider
    {
        /// <summary>
        /// Gets a list of all rooms in the databases.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Room>> QueryRooms();

        /// <summary>
        /// Gets a room in the database by id.
        /// </summary>
        /// <param name="roomId">The id of the room to get.</param>
        /// <returns></returns>
        public Task<Room> QueryRoom(long roomId);

        /// <summary>
        /// Gets a list of rooms matching the time and people specified.
        /// </summary>
        /// <param name="start">Start of the time frame to search in, null meaning no time frame.</param>
        /// <param name="end">End of the time frame to search in, null meaning no time frame.</param>
        /// <param name="minPeople">Minimum people that can fit in the room to match, with <0 including all.</param>
        /// <returns>A list of rooms matching the criteria.</returns>
        public Task<List<Room>> QueryFree(DateTime? start = null, DateTime? end = null, int minPeople = -1);

        /// <summary>
        /// Adds a room to the database. Fails if there is already a database entry with the id specified. If given a negative id, a new id will automatically be assigned.
        /// </summary>
        /// <param name="room">The room to add.</param>
        /// <returns>True on success, false on failure.</returns>
        public Task<bool> AddRoom(Room room);

        /// <summary>
        /// Removes a room by id.
        /// </summary>
        /// <param name="roomId">The id of the room to remove.</param>
        /// <returns>True on success, false on failure.</returns>
        public Task<bool> RemoveRoom(long roomId);

        /// <summary>
        /// Updates an already existing room.
        /// </summary>
        /// <param name="room">The room to modify.</param>
        /// <returns>The updated room, or null on failure.</returns>
        public Task<Room> UpdateRoom(Room room);
    }
}

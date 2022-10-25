namespace HotelReservationAPI.Model
{
    public class User
    {
        public enum EnumUserType
        {
            None=-1,
            Staff=0,
            Admin=1
        }

        public long Id { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
        public EnumUserType UserType { get; set; }
        public DateTime LastLogin { get; set; }
    }
}

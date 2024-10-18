using System.Net;

namespace SqlServerMongoApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Navigation property for related addresses
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}

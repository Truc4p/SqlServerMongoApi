namespace SqlServerMongoApi.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Foreign Key
        // public int UserId { get; set; }

        // Navigation property for the User
        // public User User { get; set; }
    }
}

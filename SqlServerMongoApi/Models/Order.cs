using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SqlServerMongoApi.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int UserId { get; set; }

        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace GameStoreMini.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        
        [JsonIgnore] // Prevent circular reference when serializing
        public Order? Order { get; set; }

        public int GameId { get; set; }
        public Game? Game { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
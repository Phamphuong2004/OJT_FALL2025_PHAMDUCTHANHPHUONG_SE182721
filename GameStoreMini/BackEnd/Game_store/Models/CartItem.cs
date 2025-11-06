using System.ComponentModel.DataAnnotations.Schema;

namespace GameStoreMini.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        public int GameId { get; set; }
        public Game? Game { get; set; }

        public int Quantity { get; set; } = 1;

        // Snapshot unit price at the time of adding to cart
        [Column(TypeName = "numeric")]
        public decimal UnitPrice { get; set; }
    }
}
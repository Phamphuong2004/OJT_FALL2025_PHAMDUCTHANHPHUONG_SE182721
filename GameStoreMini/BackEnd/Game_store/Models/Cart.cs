using System;
using System.Collections.Generic;

namespace GameStoreMini.Models
{
    // Each user can have one cart (simple model).
    public class Cart
    {
        public int Id { get; set; }
        public int? UserId { get; set; }           // nullable for guest carts
        public User? User { get; set; }
        public string? AnonymousId { get; set; }   // NEW: anonymous identifier for guest carts
        public List<CartItem> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
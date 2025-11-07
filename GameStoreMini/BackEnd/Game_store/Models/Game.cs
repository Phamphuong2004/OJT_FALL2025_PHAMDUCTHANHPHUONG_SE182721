using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Game_store.Models;

namespace GameStoreMini.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        // Quantity in stock
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        // When the game was added
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional image URL for the game
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        // Sort order for manual ordering in UI (lower = earlier)
        public int SortOrder { get; set; } = 0;

        // Many-to-many navigation to categories
        public ICollection<Category> Categories { get; set; } = new List<Category>();

        // Thêm các thuộc tính rating và reviews
        public double AverageRating { get; set; } = 0.0; // Điểm đánh giá trung bình
        public int ReviewCount { get; set; } = 0; // Số lượng đánh giá  

        // Navigation property for reviews
        public ICollection<ReviewGame> Reviews { get; set; } = new List<ReviewGame>();
    }
}
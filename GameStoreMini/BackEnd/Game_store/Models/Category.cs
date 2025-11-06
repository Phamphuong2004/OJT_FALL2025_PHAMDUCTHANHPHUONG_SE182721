using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GameStoreMini.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Optional slug for friendly URLs/lookup
        [MaxLength(100)]
        public string? Slug { get; set; }

        // Navigation
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
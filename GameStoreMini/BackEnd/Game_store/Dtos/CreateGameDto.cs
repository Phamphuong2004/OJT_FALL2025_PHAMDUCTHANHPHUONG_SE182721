using System.ComponentModel.DataAnnotations;

namespace GameStoreMini.Dtos
{
    public class CreateGameDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        // Optional category ids to assign when creating/updating a game
        public int[]? CategoryIds { get; set; }

        // Optional image URL (remote link or local path like /uploads/...)
        [MaxLength(500), Url]
        public string? ImageUrl { get; set; }
    }
}
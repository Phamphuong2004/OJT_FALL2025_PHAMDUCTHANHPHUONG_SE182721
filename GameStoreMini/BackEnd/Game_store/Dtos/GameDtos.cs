namespace GameStoreMini.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Category names for frontend convenience
        public string[] CategoryNames { get; set; } = System.Array.Empty<string>();

        // Image URL (can be remote or local /uploads/...)
        public string? ImageUrl { get; set; }
    }
}
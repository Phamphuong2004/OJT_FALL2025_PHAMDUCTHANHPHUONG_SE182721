namespace GameStoreMini.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Country { get; set; } = "";
        public string City { get; set; } = ""; // city or province name
        public string? District { get; set; }
        public string? Code { get; set; } // optional postal or admin code
    }
}

namespace BokHemsida.Models
{
    public class RatingViewModel
    {
        public int BookId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string? Review { get; set; }
    }
}

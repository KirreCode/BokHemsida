using System.ComponentModel.DataAnnotations;

namespace BokHemsida.Models
{
    public class BookViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public string UserId { get; set; }
        public int? Rating { get; set; }
        public List<Author> Authors { get; set; }
    }
}

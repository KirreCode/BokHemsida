using System.ComponentModel.DataAnnotations.Schema;

namespace BokHemsida.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public virtual Author Author { get; set; }
        public virtual List<UserBook> UserBooks { get; set; } = new List<UserBook>();
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace BokHemsida.Models
{
    public class UserBook
    {
        public int? Rating { get; set; }
        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public DateOnly DateAdded { get; set; }
    }
}

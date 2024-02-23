namespace BokHemsida.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public String? Description { get; set; }
        public virtual List<Book> Books { get; set; }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;

namespace BokHemsida.Models
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
        public DbSet<Author> Authors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = "1",
                Firstname = "Glenn",
                Lastname = "Glennartsson",
                UserName = "Glenny",
                PasswordHash = "Password!",
                Private = false,
            });
            modelBuilder.Entity<UserBook>().
            HasKey(pc => new { pc.BookId, pc.UserId });

            modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 1,
                FullName = "J.K. Rowling",
                Description = "Brittisk författare känd för att ha skrivit Harry Potter-böckerna",
                BirthDate = new DateOnly(1965, 7, 31)
            });
            modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 2,
                FullName = "J.R.R Tolkien",
                Description = "Fantasy-pionjär som har skrivit Sagan om Ringen-böckerna",
                BirthDate = new DateOnly(1892, 1, 3)
            });
            modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 3,
                FullName = "Fredrik Backman",
                Description = "Svensk författare mest känd för att ha skrivit En man som heter Ove",
                BirthDate = new DateOnly(1982, 6, 2)
            });
            modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "Harry Potter och fenixorden",
                Description = "Harry och hans vänner är ute på nya äventyr",
                AuthorId = 1,
            });
            modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 2,
                Title = "Sagan om Ringen",
                Description = "Berättelsen om härskarringen",
                AuthorId = 2,
            });
            modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 3,
                Title = "Vinnarna",
                Description = "Den tredje boken i BjörnstadsTrilogin",
                AuthorId = 3,
            });
            modelBuilder.Entity<UserBook>().HasData(
            new UserBook
            {
                BookId = 1,
                UserId = "1",
                DateAdded = new DateOnly(2005, 1, 1),
                Rating = 7,
                Review = "Jag tyckte att boken var spännande"
            });
        }
    }
}

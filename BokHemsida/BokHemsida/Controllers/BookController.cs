using BokHemsida.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;

namespace BokHemsida.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private Context _context { get; set; }
        public BookController(ILogger<BookController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Book(int id)
        {
            Book book = _context.Books.Find(id);
            return View(book);
        }
        public IActionResult AllBooks()
        {
            List<Book> book = _context.Books.ToList();
            return View(book); 
        }
        public IActionResult AddBook()
        {
            // Hämta den inloggade användarens ID
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Öppnar view och skickar med rätt objekt
            User user = _context.Users.FirstOrDefault(u => u.Id == loggedInUserId);
            List<Author> authors = _context.Authors.ToList();

            var model = new BookViewModel
            {
                UserId = user.Id,
                Authors = authors
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(BookViewModel model) 
        {
            try
            {
                // Skapar den nya utbildningsentiteten
                Book book = new Book
                {
                    Title = model.Title,
                    Description = model.Description,
                    AuthorId = model.AuthorId
                };

                UserBook userBook = new UserBook
                {
                    BookId = (_context.Books.Count() + 1),
                    UserId = model.UserId,
                    Book = book,
                };

                if (model.Rating != null)
                {
                    userBook.Rating = model.Rating;
                }

                book.UserBooks.Add(userBook);
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction("MyProfile", "User");
            }
            catch (Exception ex)
            {
                // Felhantering
                _logger.LogError(ex, "Error occurred while creating the book.");
                ModelState.AddModelError("", "Ett fel uppstod när boken skulle skapas i systemet.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangeBook(int id)
        {
            // Hämta den inloggade användarens ID
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Öppnar view och skickar med rätt objekt
            Book book = _context.Books.Find(id);
            BookViewModel bookModel = new BookViewModel() { };

            bookModel.Id = book.Id;
            bookModel.Title = book.Title;
            bookModel.Description = book.Description;
            bookModel.AuthorId = book.AuthorId;
            bookModel.UserId = loggedInUserId;
            bookModel.Authors = _context.Authors.ToList();
            

            return View(bookModel);
        }

        [HttpPost]
        public IActionResult ChangeBook(BookViewModel bookModel)
        {
            try
            {
                Book book = new Book() { };

                book.Id = (int)bookModel.Id;
                book.Title = bookModel.Title;
                book.Description = bookModel.Description;
                book.AuthorId = bookModel.AuthorId;

                _context.Books.Update(book);
                _context.SaveChanges();
                return RedirectToAction("MyProfile", "User");
            }
            catch
            {
                //Någon form av logger och exceptionhandling
                return RedirectToAction("MyProfile", "User");
            }
        }

        [HttpGet]
        public IActionResult DeleteBook(int id)
        {
            // Hämta den inloggade användarens ID
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            //Öppnar "Bekräfta borttagnings"-sida
            Book book = _context.Books.Find(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult DeleteB(int id)
        {
            //Tar bort valt objekt 
            Book book = _context.Books.Find(id);

            if (book != null)
            {
                _context.Remove(book);
            }
            _context.SaveChanges();
            return RedirectToAction("MyProfile", "User");
        }

        public IActionResult AddRating(int id)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                RatingViewModel ratingViewModel = new RatingViewModel();

                Book book = _context.Books.Find(id);

                ratingViewModel.BookId = book.Id;
                ratingViewModel.UserId = userId;

                return View(ratingViewModel);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRating(RatingViewModel ratingViewModel)
        {
            Book book = _context.Books.Find(ratingViewModel.BookId);
            UserBook userBook = new UserBook();
            userBook.Rating = ratingViewModel.Rating;
            userBook.UserId = ratingViewModel.UserId;
            userBook.BookId = ratingViewModel.BookId;
            userBook.Book = book;

            if(ratingViewModel.Review != null)
            {
                userBook.Review = ratingViewModel.Review;
            }

            try
            {
                book.UserBooks.Add(userBook);
                _context.UserBooks.Add(userBook);

                await _context.SaveChangesAsync();

                return RedirectToAction("AllBooks", "Book");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the rating.");

                ModelState.AddModelError("", "Ett fel uppstod när din rating skulle skapas i systemet.");
            }

            return View(ratingViewModel);
        }

        [HttpGet]
        public IActionResult ChangeRating(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            UserBook userBook = _context.UserBooks
                .FirstOrDefault(ub => ub.UserId == userId && ub.BookId == bookId);
            return View(userBook);
        }

        [HttpPost]
        public IActionResult ChangeRating(UserBook userbook)
        {
            try
            {
                _context.UserBooks.Update(userbook);
                _context.SaveChanges();
                return RedirectToAction("MyProfile", "User");
            }
            catch
            {
                //Någon form av logger och exceptionhandling
                return RedirectToAction("MyProfile", "User");
            }
        }
    }
}

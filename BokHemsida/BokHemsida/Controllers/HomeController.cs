using BokHemsida.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace BokHemsida.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Context _context { get; set; }

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            String Id = "1";
            List<Book> books = _context.Books.ToList();
            return View(books);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search(string search)
        {

            List<User> matchedUsers = new List<User>();
            List<Book> matchedBooks = new List<Book>();
            List<Author> matchedAuthors = new List<Author>();

            if (string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index");
            }

            matchedUsers = _context.Users
            .Where(user =>
                (user.Firstname.Contains(search) || user.Lastname.Contains(search)))
            .Select(user => user).ToList();

            matchedBooks = _context.Books
            .Where(book =>
                (book.Title.Contains(search)))
            .Select(book => book).ToList();

            matchedAuthors = _context.Authors
            .Where(author =>
                (author.FullName.Contains(search)))
            .Select(author => author).ToList();

            SearchViewModel searchViewModel = new SearchViewModel();

            searchViewModel.Users = matchedUsers;
            searchViewModel.Books = matchedBooks;
            searchViewModel.Authors = matchedAuthors;

            return View("SearchResult", searchViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

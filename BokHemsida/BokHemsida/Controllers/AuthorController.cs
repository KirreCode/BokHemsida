using BokHemsida.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BokHemsida.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ILogger<AuthorController> _logger;
        private Context _context { get; set; }
        public AuthorController(ILogger<AuthorController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AllAuthors()
        {
            List<Author> authors = _context.Authors.ToList();
            return View(authors);
        }
        public IActionResult AddAuthor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAuthor(Author author)
        {
            try
            {
                _context.Authors.Add(author);

                await _context.SaveChangesAsync();

                return RedirectToAction("AllAuthors", "Author");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the author.");

                ModelState.AddModelError("", "Ett fel uppstod när författaren skulle skapas i systemet.");
            }

            return View(author);
        }

        [HttpGet]
        public IActionResult ChangeAuthor(int id)
        {
            //Öppnar view och skickar med rätt objekt
            Author author = _context.Authors.Find(id);
            return View(author);
        }

        [HttpPost]
        public IActionResult ChangeAuthor(Author author)
        {
            try
            {
                _context.Authors.Update(author);
                _context.SaveChanges();
                return RedirectToAction("AllAuthors", "Author");
            }
            catch
            {
                //Någon form av logger och exceptionhandling
                return RedirectToAction("AllAuthors", "Author");
            }
        }

        [HttpGet]
        public IActionResult DeleteAuthor(int id)
        {
            //Öppnar "Bekräfta borttagnings"-sida
            Author author = _context.Authors.Find(id);
            return View(author);
        }

        [HttpPost]
        public IActionResult DeleteAuth(int id)
        {
            //Tar bort valt objekt 
            Author author = _context.Authors.Find(id);

            if (author != null)
            {
                _context.Remove(author);
            }
            _context.SaveChanges();
            return RedirectToAction("AllAuthors", "Author");
        }
    }
}

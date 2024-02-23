using BokHemsida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BokHemsida.Controllers
{
    public class UserController : Controller
    {
        private Context _context { get; set; }
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, Context context, ILogger<UserController> loggerUser)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult MyProfile()
        {
            // Hämta den inloggade användarens ID
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Hämta antalet olästa meddelanden där den inloggade användaren är mottagaren och läggs i en ViewBag.
            //int unreadMessagesCount = _context.Message_User
            //    .Count(mu => mu.ReceiverId == loggedInUserId && !mu.Message.Read);

            //ViewBag.Count = unreadMessagesCount;

            User user = _context.Users.FirstOrDefault(u => u.Id == loggedInUserId);
            return View(user);
        }
    }
}

using BokHemsida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public IActionResult ChangeUser(string id)
        {
            // Hämta den inloggade användarens ID
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            User user = _context.Users.Find(id);
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.Firstname = user.Firstname;
            registerViewModel.Lastname = user.Lastname;
            registerViewModel.Password = user.PasswordHash;

            return View(registerViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUser(RegisterViewModel updatedUser)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _context.Users.FirstOrDefault(u => u.Id == loggedInUserId);
            user.Firstname = updatedUser.Firstname;
            user.Lastname = updatedUser.Lastname;
            if (updatedUser.Username != null)
            {
                var existingUser = await userManager.FindByNameAsync(updatedUser.Username);
                if (existingUser != null)
                {
                    ModelState.Remove("Id");
                    ModelState.AddModelError("", "Användarnamnet är redan upptaget. Välj ett annat användarnamn.");
                    return View(updatedUser);
                }
                else
                {
                    user.UserName = updatedUser.Username;
                    await userManager.UpdateNormalizedUserNameAsync(user);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("MyProfile", "User");
        }
    }
}

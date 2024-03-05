using BokHemsida.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BokHemsida.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private Context _context { get; set; }
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, Context context, ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            LogInViewModel logInViewModel = new LogInViewModel();
            return View(logInViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel logInViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(logInViewModel.Username, logInViewModel.Password,
                    isPersistent: logInViewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Felaktigt användarnamn eller lösenord ");

                }
            }
            return View(logInViewModel);
        }

        //Returnerar vyn för inlogg
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //Sköter logiken för att registrera en användare
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User();

                user.Firstname = registerViewModel.Firstname;
                user.Lastname = registerViewModel.Lastname;
                user.UserName = registerViewModel.Username;

                var passwordIsValid = ValidatePassword(registerViewModel.Password);
                if (!passwordIsValid)
                {
                    ModelState.AddModelError("", "Lösenordet uppfyller inte kraven.");
                    return View(registerViewModel);
                }

                var existingUser = await userManager.FindByNameAsync(registerViewModel.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Användarnamnet är redan upptaget. Välj ett annat användarnamn.");
                    return View(registerViewModel);
                }

                var result = await userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "PasswordTooShort")
                        {
                            ModelState.AddModelError("", "Lösenordet måste vara minst 8 tecken långt.");
                        }
                        else if (error.Code == "PasswordRequiresDigit")
                        {
                            ModelState.AddModelError("", "Lösenordet måste innehålla minst en siffra.");
                        }
                        else
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return View(registerViewModel);
        }

        //Validera Lösenord
        private bool ValidatePassword(string password)
        {
            // Kontrollera längd
            if (password.Length < 8)
            {
                ModelState.AddModelError("", "Lösenordet måste vara minst 8 tecken långt.");
                return false;
            }

            // Kontrollera stor bokstav
            if (!password.Any(char.IsUpper))
            {
                ModelState.AddModelError("", "Lösenordet måste innehålla minst en stor bokstav.");
                return false;
            }

            // Kontrollera liten bokstav
            if (!password.Any(char.IsLower))
            {
                ModelState.AddModelError("", "Lösenordet måste innehålla minst en liten bokstav.");
                return false;
            }

            // Kontrollera siffra
            if (!password.Any(char.IsDigit))
            {
                ModelState.AddModelError("", "Lösenordet måste innehålla minst en siffra.");
                return false;
            }

            // Kontrollera specialtecken
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ModelState.AddModelError("", "Lösenordet måste innehålla minst ett specialtecken.");
                return false;
            }

            // Lösenordet uppfyller alla krav
            return true;
        }
    }
}

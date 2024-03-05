using ASP.NET_CVProjekt.Models;
using BokHemsida.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BokHemsida.Controllers
{
    public class PictureController : Controller
    {
        private Context _context { get; set; }

        public PictureController(Context context)

        {
            _context = context;
        }

        //Hämtar bilden som ska läggas upp

        [HttpGet]
        public IActionResult AddPicture()
        {
            NewPicture newPicture = new NewPicture();
            return View(newPicture);
        }
        //Än så länge oanvänd controller
    }
}

using API.Use;
using Domain.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        public IActionResult Report()
        {
            return View();
        }


        public async Task<IActionResult> Index()
        {

            var result = await new Http().GetJson("/api/MenuAPI/List", new List<Menu>());
            result = result ?? new List<Menu>();
            List<Menu> menus = (List<Menu>)result;

            ViewBag.success = true;
            return View(menus);
        }
    }
}

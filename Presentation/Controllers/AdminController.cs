using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }



        public async Task<IActionResult> users()
        {

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://localhost:7149/api/Authentication");
            var jsonString = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<IdentityUser>>(jsonString);

            ViewBag.userManager = _userManager;
            return View(values);
        }

    }
}

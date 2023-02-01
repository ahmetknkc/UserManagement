using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Presentation.Controllers
{

    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string mail { get; set; }
        public List<string> Roles { get; set; }
    }


    public class AdminController : Controller
    {

        public async Task<IActionResult> users()
        {

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://localhost:7149/api/Authentication");
            var jsonString = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<IdentityUser>>(jsonString);


            response = await httpClient.GetAsync("https://localhost:7149/api/Role");
            jsonString = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<UserRolesViewModel>>(jsonString);

            ViewBag.roles = roles;

            return View(values);
        }

    }
}

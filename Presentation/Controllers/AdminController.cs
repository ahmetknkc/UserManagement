using API.Use;
using Domain.Models.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Presentation.Controllers
{


    public class AdminController : Controller
    {


        public async Task<IActionResult> Users()
        {

            //var httpClient = new HttpClient();
            //var response = await httpClient.GetAsync($"https://localhost:{Program.swaggerPort}/api/ManageUser");
            //var jsonString = await response.Content.ReadAsStringAsync();
            //var users = JsonConvert.DeserializeObject<List<UserWithRoles>>(json);

            object? json = await new Http().GetJson("/api/UserAPI", new List<UserWithRoles>());
            Console.WriteLine(json);
            return View(json);
        }



        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            object? userData = await new Http().GetJson($"/api/UserAPI/{id}", new UserWithRoles());
            object? roles = await new Http().GetJson($"/api/RoleAPI/", new List<IdentityRole>());

            List<IdentityRole> viewBagRoles = new();
            foreach (var item in (List<IdentityRole>)roles)
            {
                viewBagRoles.Add(item);
            }

            ViewData["roles"] = viewBagRoles;

            return View(userData);
        }

    }
}

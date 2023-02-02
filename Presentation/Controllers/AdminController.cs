using API.Use;
using Domain;
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

            object? json = await new Http().GetJson("/api/ManageUser", new List<UserWithRoles>());
            Console.WriteLine(json);
            return View(json);
        }



        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            object? json = await new Http().GetJson($"/api/ManageUser/{id}", new UserWithRoles());
            return View(json);
        }

    }
}

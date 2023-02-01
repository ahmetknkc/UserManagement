using Domain.Models.Authentication.SignUp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;

namespace Presentation.Controllers
{
    public class AuthController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser rUser)
        {

            var httpClient = new HttpClient();
            var toJson = JsonConvert.SerializeObject(rUser);
            StringContent content = new StringContent(toJson, Encoding.UTF8, "application/json");


            var response = await httpClient.PostAsync(@"https://localhost:7149/api/Authentication", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Home");
            }
            else
            {
                Console.WriteLine("_Err: " + response.ToString());
                return View();
            }
        }
    }
}

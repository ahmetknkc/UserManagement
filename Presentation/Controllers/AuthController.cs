using API.Use;
using Application.ValidationRules;
using Domain.Models.Authentication.Login;
using Domain.Models.Authentication.SignUp;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.Models;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Presentation.Controllers
{


    [AllowAnonymous]
    public class AuthController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IAuthorizationService authorizationService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }




        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUser());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUser rUser)
        {
            RegisterValidator validator = new RegisterValidator();
            ValidationResult results = validator.Validate(rUser);

            if (results.IsValid)
            {
                if (await _userManager.FindByNameAsync(rUser.username) != null)
                {
                    if (await _userManager.FindByEmailAsync(rUser.email) != null)
                        ModelState.AddModelError(nameof(rUser.email), "E-Mail already taken.");
                    ModelState.AddModelError(nameof(rUser.username), "Username already taken.");
                    return View(rUser);
                }

                await new Http().PostJson("/api/UserAPI", rUser);
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    Console.WriteLine(item.ErrorMessage);
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View(rUser);
            }
        }


        [HttpGet]
        public IActionResult Login()
        {
            var isLogin = _signInManager.IsSignedIn(User);
            if (isLogin)
                return RedirectToAction("Index", "Home");
            return View(new LoginModel() { Password = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login /*, string returnUrl */)
        {
            ViewBag.ReturnUrl = "";
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                ViewBag.err = true;
                return View(login);
            }


            IdentityUser user = await _userManager.FindByNameAsync(login.Username);

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, true, false);
            if (result.Succeeded)
            {
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);

                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                foreach (var role in userRoles)
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                identity.AddClaims(authClaims);

                return RedirectToAction("Index", "Home");
            }


            ViewBag.err = true;
            return View(login);
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

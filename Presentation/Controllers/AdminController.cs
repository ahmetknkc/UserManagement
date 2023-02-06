using API.Use;
using Domain.Models.Authentication.SignUp;
using Domain.Models.EntityFramework;
using Domain.Models.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace Presentation.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthorizationService _authorizationService;

        public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
        }



        public async Task<IActionResult> Index()
        {
            return View();
        }



        #region Roles
        #region Querys
        [Authorize(Roles = "Admin")]
        [Route("Admin/DeleteMenuAndRole/{menuID}")]
        public async Task<IActionResult> DeleteMenuAndRole(string menuID)
        {
            Console.WriteLine(menuID);
            await new Http().PostJson<Menu>($"/api/MenuAPI/Remove", new Menu() { ID = menuID });
            return RedirectToAction("Roles", "Admin");
        }
        #endregion

        #region Pages
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Roles()
        {
            object? result = await new Http().GetJson("/api/MenuAPI/List", new List<Menu>());

            Console.WriteLine(result);

            List<Menu> menus = new();
            foreach (var item in (List<Menu>?)result ?? new List<Menu>())
            {
                Console.WriteLine(item.MenuName);
                menus.Add(item);
            }

            ViewBag.menus = menus;
            return View(new Menu());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Roles(Menu menu)
        {
            //https: //localhost:7149/api/MenuAPI/Remove
            menu.ID = Guid.NewGuid().ToString();


            foreach (var item in _roleManager.Roles)
            {
                if (item.Name == menu.AuthorizeRole)
                {
                    object? menuList = await new Http().GetJson("/api/MenuAPI/List", new List<Menu>());
                    ViewData["err-Duplicated"] = true;
                    ViewBag.menus = (List<Menu>)menuList;
                    return View(new Menu());
                }
            }

            object? result = await new Http().PostJson("/api/MenuAPI/Create", menu);


            var id = ((JObject)result)["id"];

            return RedirectToAction($"Category", "Admin", new { categoryID = id });
        }

        #endregion
        #endregion

        #region User
        #region Query's
        [Authorize(Roles = "Admin")]
        [Route("Admin/UserToggleRole/{userID}/{role}")]
        public async Task<IActionResult> UserToggleRole(string userID, string role)
        {

            Console.Clear();
            var user = await _userManager.FindByIdAsync(userID);
            Console.WriteLine(user.UserName);
            bool isHaveRole = await _userManager.IsInRoleAsync(user, role);
            string apiUrl = "GiveRole";
            Console.WriteLine("----Is Have Role: " + isHaveRole.ToString());
            if (isHaveRole)
                apiUrl = "TakeRole";
            await new Http().PostJson<UserIdAndRoleId>($"/api/RoleAPI/{apiUrl}", new UserIdAndRoleId() { UserId = userID, RoleName = role });

            if (isHaveRole)
            {
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                identity.RemoveClaim(new Claim(ClaimTypes.Role, role));
            }
            return Ok();
        }



        [Authorize(Roles = "Admin")]
        [Route("Admin/User/SaveChanges/")]
        [HttpPost]
        public async Task<IActionResult> SaveChanges(UserWithRoles rUser)
        {
            Console.WriteLine("--------------Logout");
            Console.WriteLine(rUser.User.UserName);
            Console.WriteLine(rUser.User.Email);


            var user = await _userManager.FindByIdAsync(rUser.User.Id);
            await _userManager.UpdateSecurityStampAsync(user);

            await _userManager.SetEmailAsync(user, rUser.User.Email);
            await _userManager.SetUserNameAsync(user, rUser.User.UserName);

            return RedirectToAction("EditUser", "Admin", new { id = rUser.User.Id });
        }



        [Authorize(Roles = "Admin")]
        [Route("Admin/DeleteUser/{ID}")]
        public async Task<IActionResult> DeleteUser(string ID)
        {
            await new Http().PostJson($"/api/UserAPI/Delete/{ID}", ID);
            return RedirectToAction("Users", "Admin");
        }

        #endregion

        #region Pages
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            object? json = await new Http().GetJson("/api/UserAPI", new List<UserWithRoles>());
            Console.WriteLine(json);
            return View(json);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            object? userData = await new Http().GetJson($"/api/UserAPI/{id}", new UserWithRoles());
            object? roles = await new Http().GetJson($"/api/RoleAPI/", new List<IdentityRole>());




            List<IdentityRole> viewBagRoles = new();
            foreach (var item in (List<IdentityRole>?)roles ?? new List<IdentityRole>())
            {
                viewBagRoles.Add(item);
            }

            ViewData["roles"] = viewBagRoles;

            return View(userData);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string id, bool temp = true)
        {


            return View();
        }

        //[Authorize(Roles = "Admin")]
        //[Route("Admin/UserGiveRole/{userID}/{role}")]
        //public async Task<IActionResult> UserGiveRole(string userID, string role)
        //{
        //    await new Http().PostJson<GiveRole>("/api/RoleAPI/GiveRole", new GiveRole() { userId = userID, roleName = role });
        //    return Ok();
        //}

        //[Authorize(Roles = "Admin")]
        //[Route("Admin/UserTakeRole/{userID}/{role}")]
        //public async Task<IActionResult> UserTakeRole(string userID, string role)
        //{
        //    await new Http().PostJson<GiveRole>("/api/RoleAPI/TakeRole", new GiveRole() { userId = userID, roleName = role });
        //    return Ok();
        //}
        #endregion

        #endregion


        #region Category

        #region Methods
        private async Task<bool> CheckAuthorize(string role)
        {
            IdentityUser user = await _userManager.FindByNameAsync(User?.Identity?.Name ?? "");
            if (user == null)
                return false;
            return await _userManager.IsInRoleAsync(user, role);
        }
        #endregion


        [Route("Admin/Category/{categoryID}")]
        public async Task<IActionResult> Category(string categoryID)
        {
            object? menuList = await new Http().GetJson("/api/MenuAPI/List", new List<Menu>());

            Menu category = ((List<Menu>)menuList)?.Find(x => x.ID == categoryID) ?? new Menu() { ID = string.Empty };
            if (category == null)
                return NotFound();
            if (!await CheckAuthorize(category.AuthorizeRole) && !await CheckAuthorize("Admin"))
                return Forbid();

            return View(category);
        }


        [HttpPost]
        [Route("Admin/Category/{categoryID}")]
        public async Task<IActionResult> Category(Menu menu)
        {
            await new Http().PostJson("/api/MenuAPI/Update", menu);
            ViewBag.success = true;
            return View(menu);
        }

        #endregion
    }
}

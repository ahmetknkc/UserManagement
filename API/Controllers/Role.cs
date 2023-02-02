using Application.Services;
using Domain.Models.Authentication.SignUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace API.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class Role : ControllerBase
    {

        private readonly IRoleService _roleService;

        public Role(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoles()
        {
            return Ok(_roleService.GetRolesNameAsync());
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoles()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    var roles = await _roleManager.Roles.ToListAsync();

        //    var result = users.Select(user => new
        //    {
        //        UserId = user.Id,
        //        UserName= user.UserName,
        //        mail = user.Email,
        //        Roles = _userManager.GetRolesAsync(user).Result
        //    });

        //    return Ok(result);
        //}

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRole()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    var roles = await _roleManager.Roles.ToListAsync();

        //    var result = users.Select(user => new
        //    {
        //        user = user,
        //        Roles = _userManager.GetRolesAsync(user).Result
        //    });

        //    return Ok(result);
        //}
    }
}

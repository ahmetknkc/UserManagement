using Application.Services;
using Domain.Models.Authentication.SignUp;
using Domain.Models.Swagger;
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
    public class RoleAPI : ControllerBase
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public RoleAPI(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IRoleService roleService, IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleService = roleService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoles()
        {
            return Ok(_roleService.GetRolesNameAsync());
        }

        [HttpDelete("RemoveUserRole")]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> RemoveUserRole([FromBody] UserIdAndRoleId? IDs)
        {
            await _roleService.DeleteUserRole(IDs?.UserId ?? "", IDs?.RoleName ?? "");
            return Ok();
        }

        [HttpPost("GiveRole")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GiveRole([FromBody] UserIdAndRoleId? IDs)
        {
            IdentityUser user = await _userManager.FindByIdAsync(IDs?.UserId);
            if (user is null) return BadRequest();
            await _roleService.GiveRole(IDs?.UserId ?? "", IDs?.RoleName ?? "");
            return Ok();
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> CreateRole([FromBody] string roleName)
        {
            await _roleService.CreateRole(roleName);
            return Ok();
        }

    }
}

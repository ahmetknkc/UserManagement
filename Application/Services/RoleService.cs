using Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    #region Interface
    public interface IRoleService
    {
        Task<IActionResult> DeleteUserRole(string userId, string roleName);
        Task<IActionResult> GiveRole(string userId, string roleName);
        Task<IActionResult> CreateRole(string roleName);


         IQueryable<IdentityRole> GetRolesNameAsync();
    }

    #endregion




    public class RoleService : ControllerBase, IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteUserRole(string userId, string roleName)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            var result = _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Result.Succeeded)
                return Ok();

            return BadRequest();
        }

        public IQueryable<IdentityRole> GetRolesNameAsync()
        {
            return _roleManager.Roles;
        }

        public async Task<IActionResult> GiveRole(string userId, string roleName)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);

            await _userManager.AddToRoleAsync(user, roleName);
            return Ok();
        }

        public async Task<IActionResult> CreateRole(string roleName)
        {
            IdentityRole roleExists = await _roleManager.FindByNameAsync(roleName);

            if (roleExists != null)
            {
                Console.WriteLine("__ Bad Request");
                return BadRequest();
            }
            await _roleManager.CreateAsync(new IdentityRole() { Name = roleName });
            return Ok();
        }

    }
}

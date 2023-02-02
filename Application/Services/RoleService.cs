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
        //Task<IdentityRole> GetRoleAsync(int id);
        //Task<IdentityRole> CreateRoleAsync(IdentityRole role);
        //Task UpdateRoleAsync(IdentityRole role);
        //Task DeleteRoleAsync(int id);


        IQueryable<IdentityRole> GetRolesNameAsync();
    }

    #endregion
    public class RoleService : ControllerBase, IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IQueryable<IdentityRole> GetRolesNameAsync()
        {
            return _roleManager.Roles;
        }


        //    public async Task<IEnumerable<Role>> GetRolesAsync()
        //    {
        //        return await _roleManager.Roles.ToListAsync();
        //    }

        //    public async Task<Role> GetRoleAsync(int id)
        //    {
        //        return await _roleManager.FindByIdAsync(id.ToString());
        //    }

        //    public async Task<Role> CreateRoleAsync(Role role)
        //    {
        //        var result = await _roleManager.CreateAsync(role);
        //        if (!result.Succeeded)
        //        {
        //            throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        //        }
        //        return role;
        //    }

        //    public async Task UpdateRoleAsync(Role role)
        //    {
        //        var result = await _roleManager.UpdateAsync(role);
        //        if (!result.Succeeded)
        //        {
        //            throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        //        }
        //    }

        //    public async Task DeleteRoleAsync(int id)
        //    {
        //        var role = await _roleManager.FindByIdAsync(id.ToString());
        //        var result = await _roleManager.DeleteAsync(role);
        //        if (!result.Succeeded)
        //        {
        //            throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        //        }
        //    }
    }
}

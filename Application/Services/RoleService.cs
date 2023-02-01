//using Domain.Models.Authentication;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Services
//{
//    #region Interface
//    public interface IRoleService
//    {
//        Task<IEnumerable<Role>> GetRolesAsync();
//        Task<Role> GetRoleAsync(int id);
//        Task<Role> CreateRoleAsync(Role role);
//        Task UpdateRoleAsync(Role role);
//        Task DeleteRoleAsync(int id);
//    }

//    #endregion
//    public class RoleService : IRoleService
//    {
//        private readonly RoleManager<Role> _roleManager;

//        public RoleService(RoleManager<Role> roleManager)
//        {
//            _roleManager = roleManager;
//        }

//        public async Task<IEnumerable<Role>> GetRolesAsync()
//        {
//            return await _roleManager.Roles.ToListAsync();
//        }

//        public async Task<Role> GetRoleAsync(int id)
//        {
//            return await _roleManager.FindByIdAsync(id.ToString());
//        }

//        public async Task<Role> CreateRoleAsync(Role role)
//        {
//            var result = await _roleManager.CreateAsync(role);
//            if (!result.Succeeded)
//            {
//                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
//            }
//            return role;
//        }

//        public async Task UpdateRoleAsync(Role role)
//        {
//            var result = await _roleManager.UpdateAsync(role);
//            if (!result.Succeeded)
//            {
//                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
//            }
//        }

//        public async Task DeleteRoleAsync(int id)
//        {
//            var role = await _roleManager.FindByIdAsync(id.ToString());
//            var result = await _roleManager.DeleteAsync(role);
//            if (!result.Succeeded)
//            {
//                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
//            }
//        }
//    }
//}

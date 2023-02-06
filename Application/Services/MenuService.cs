using Domain.Models.EntityFramework;
using Infrastructure.Context;
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

    public interface IMenuService
    {

        List<Menu> ListMenu();
        Task<IActionResult> CreateMenu(Menu newMenu);
        Task<IActionResult> RemoveMenu(Menu newMenu);
        Task<IActionResult> UpdateMenu(Menu newMenu);
    }
    public class MenuService : ControllerBase, IMenuService
    {
        private readonly EfDbContext _context;
        private readonly IRoleService _roleService;
        private readonly RoleManager<IdentityRole> _roleManager;


        public MenuService(EfDbContext context, IRoleService roleService, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleService = roleService;
            _roleManager = roleManager;
        }

        public List<Menu> ListMenu()
        {
            return _context.Menu.ToList();
        }

        public async Task<IActionResult> CreateMenu(Menu newMenu)
        {
            newMenu.ID = Guid.NewGuid().ToString();
            _context.Menu.Add(newMenu);

            await _roleService.CreateRole(newMenu?.AuthorizeRole ?? "");
            await _context.SaveChangesAsync();

            return Ok(newMenu);
        }

        public async Task<IActionResult> RemoveMenu(Menu menu)
        {
            var foundedMenu = _context.Menu.Find(menu.ID);
            var role = await _roleManager.FindByNameAsync(foundedMenu.AuthorizeRole);
            _context.Menu.Remove(foundedMenu);
            await _roleManager.DeleteAsync(role);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> UpdateMenu(Menu newMenu)
        {
            _context.Menu.Update(newMenu);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

    }
}

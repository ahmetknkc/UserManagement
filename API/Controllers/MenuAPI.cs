using Application.Services;
using Domain.Models.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuAPI : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuAPI(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("List")]
        public IActionResult GetList()
        {
            return Ok(_menuService.ListMenu());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Menu menu)
        {
            return await _menuService.CreateMenu(menu);
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> Remove([FromBody] Menu menu)
        {
            return Ok(await _menuService.RemoveMenu(menu));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Menu menu)
        {
            return Ok(await _menuService.UpdateMenu(menu));
        }



    }
}

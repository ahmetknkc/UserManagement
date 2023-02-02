using Application.Services;
using Domain.Models;
using Domain.Models.Authentication.SignUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        #region Props And Const
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _userService = userService;
        }
        #endregion



        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser /*, string role = null*/ )
        {
            var result = await _userService.CreateUserAsync(registerUser);
            if (result is OkResult)
            {
                return StatusCode(StatusCodes.Status201Created, new Response
                {
                    Status = "Created",
                    Message = "User Created Successfully"
                });
            }
            else if (result is BadRequestObjectResult badRequest)
            {

                Console.WriteLine(badRequest.Value?.ToString() ?? "badRequest is null.");

                //return BadRequest(badRequest.Value.ToString());
                var errorMessage = JObject.Parse(badRequest.Value?.ToString());
                if (errorMessage != null)
                {
                    Console.WriteLine("errorMessage is not null.");
                    return BadRequest(errorMessage["Message"].Value<string>());
                }
            }

            return BadRequest("Unknown error.");
        }



        [HttpGet]
        public  async Task<ActionResult<IEnumerable<RegisterUser>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var result = users.Select(user => new
            {
                user = user,
                Roles = _userManager.GetRolesAsync(user).Result
            });

            return Ok(result);
            //return Ok(_userManager.Users);
        }






        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await _userService.GetUserAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser(string id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }
        //    await _userService.UpdateUserAsync(user);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    await _userService.DeleteUserAsync(id);
        //    return NoContent();
        //}



    }
}

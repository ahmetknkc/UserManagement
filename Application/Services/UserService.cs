using Domain.Models;
using Domain.Models.Authentication.SignUp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Domain.Models.Swagger;

namespace Application.Services
{

    #region Interface
    public interface IUserService
    {
        Task<IEnumerable<IdentityUser>> GetUsersAsync();
        Task<UserWithRoles> GetUserAsync(string id);
        Task<IActionResult> CreateUserAsync(RegisterUser user);
        Task<IdentityUser> Login(string username, string password);
        Task UpdateUserAsync(IdentityUser user);
        Task DeleteUserAsync(string id);
    }
    #endregion

    public class UserService : ControllerBase, IUserService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserService(UserManager<IdentityUser> identityUserManager,
                    RoleManager<IdentityRole> roleManager,
                    SignInManager<IdentityUser> signInManager)
        {
            _userManager = identityUserManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }



        public async Task<IEnumerable<IdentityUser>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<UserWithRoles> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = _userManager.GetRolesAsync(user).Result.ToList();
            var result = new UserWithRoles
            {
                User = user,
                Roles = roles
            };

            return result;


        }

        public async Task<IActionResult> CreateUserAsync(RegisterUser registerUser)
        {

            var userExist = await _userManager.FindByEmailAsync(registerUser.email);

            if (userExist != null)
            {
                var jsonResult = JsonConvert.SerializeObject(new Response { Status = "Error", Message = "E-Mail already exists." });
                return new BadRequestObjectResult(jsonResult);
            }

            IdentityUser? user = new()
            {
                Email = registerUser.email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.username
            };

            var result = await _userManager.CreateAsync(user, registerUser.password);
            if (!result.Succeeded)
            {
                string error = String.Join("\r\n", (from x in result.Errors
                                                    select $"{x.Description}"));

                return new BadRequestObjectResult(new { message = error });
            }
            await _userManager.AddToRoleAsync(user, "User");
            return Ok();
        }



        public async Task<IdentityUser> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            Console.WriteLine($"Login Username: {username}");
            Console.WriteLine($"Login Password: {password}");
            if (user != null)
            {
                Console.WriteLine("Login: user is not null.");

                var result = await _signInManager.CanSignInAsync(user);

                if (result)
                {

                    var loginResult = await _signInManager.PasswordSignInAsync(user, password, false, true);
                    Console.WriteLine("Login: Login success.");
                    if (loginResult.Succeeded)
                        return user;
                }
            }
            Console.WriteLine("Login: Bad Req.");
            return null;
        }



        public async Task UpdateUserAsync(IdentityUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }


    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Pharmacy.Entities;
using System.Web;
using Pharmacy.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accService;
        private readonly IJWTService _jWTManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
                                IAccountService accService, IJWTService jWTManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _accService = accService;
            _jWTManager = jWTManager;
            //_hostUrl = options.Value;
        }

        // POST api/Account/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(Register model)
        {
            await CheckRoles();

            string email = await _accService.GetUsername(model.Email);

            if (email == null)
            {
                if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        UserName = model.Email,
                        Surname = model.Surname,
                        FatherName = model.Fathername,
                        Email = model.Email,
                        Name = model.Name,
                        RegisteredOn = DateTime.Now, 
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, model.RoleName);

                        try
                        {
                            await _accService.SaveAllAsync();
                        }
                        catch (Exception)
                        {
                            await _userManager.DeleteAsync(user);
                            throw;
                        }

                        return NoContent();
                    }

                    return BadRequest("Register failed");

                }
            }

            return BadRequest("Email is already exists");
        }

        // POST api/Account/authenticate
        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            var roles = await _userManager.GetRolesAsync(user);

            if (user.EmailConfirmed == true)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);

                var token = _jWTManager.Authenticate(user.Id, user.Name, roles);

                if (token == null)
                {
                    return Unauthorized();
                }

                user.LastActive = DateTime.Now;
                await _accService.SaveAllAsync();

                return Ok(token);
            }

            else return BadRequest("Your email hasn't been confirmed");
        }

        // GET api/Account/roles
        [HttpGet("roles")]
        public async Task<IActionResult> CheckRoles()
        {
            var roleList = await _accService.GetRoles();

            return Ok(roleList);
        }

        // GET: api/Account/users/id
        [HttpGet]
        [Route("users/{id}/{role}")]
        public async Task<IActionResult> GetUserById(string id, string role)
        {
            User user = await _accService.GetUserById(id, role);

            return Ok(user);
        }
    }
}

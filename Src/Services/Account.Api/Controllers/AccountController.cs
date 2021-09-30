using Account.Api.DTO;
using Account.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Account.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<MyUser> userManager;
        private readonly SignInManager<MyUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(IConfiguration configuration, UserManager<MyUser> userManager, 
                    SignInManager<MyUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            if (!await UserExists(login.UserName.ToLower()))
            {
                return Unauthorized();
            }

            var user = await userManager.Users.SingleAsync(x => x.UserName == login.UserName);

            var result = await signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded) return Unauthorized();

            string token = await GetToken(user);

            return new UserDTO()
            {
                Token = token,
                UserName = user.UserName
            };

        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            if (await UserExists(register.UserName))
            {
                return BadRequest("Ya existe el usuario");
            }

            var user = new MyUser()
            {
                UserName = register.UserName,
                Email = register.Email,
                Puesto = register.Puesto
            };

            var result = await userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return new UserDTO()
            {
                UserName = user.UserName,
                Token = await GetToken(user)
            };
        }

        [HttpPost("Roles")]
        public async Task<ActionResult> CreateRole(string role)
        {
            await roleManager.CreateAsync(new IdentityRole { Name = role });
            return NoContent();
        }

        [HttpPost("AddUserToRole")]
        public async Task<ActionResult> AddUserToRole(string userName, string role)
        {
            var user = await userManager.FindByNameAsync(userName);
            var result = await userManager.AddToRoleAsync(user, role);
            return NoContent();
        }

        private async Task<string> GetToken(MyUser user)
        {
            var now = DateTime.UtcNow;
            var key = configuration.GetValue<string>("Identity:key");
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var signinKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            var roles = await userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var encodedJwt = new JwtSecurityTokenHandler();

            var token = encodedJwt.CreateToken(tokenDescriptor);

            return encodedJwt.WriteToken(token);
        }

        private async Task<bool> UserExists(string userName) 
            => await userManager.Users.AnyAsync(x => x.UserName == userName);
    }
}

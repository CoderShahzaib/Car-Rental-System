
using CarRentalSystem.Core.DTOs;
using CarRentalSystem.Core.Identity;
using CarRentalSystem.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.WebAPI.Controllers
{
    public class AccountController : CustomControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwt;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJwtService jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister([FromBody] RegisterDTO registerUserDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return Problem(errorMessage);
            }

            ApplicationUser user = new ApplicationUser()
            {
                PersonName = registerUserDTO.PersonName,
                UserName = registerUserDTO.PersonName,

            };

            IdentityResult result = await _userManager.CreateAsync(user, registerUserDTO.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                var authenticationResponse = _jwt.CreateJwtToken(user);
                user.RefreshToken = authenticationResponse.RefreshToken;
                user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
                await _userManager.UpdateAsync(user);
                return Ok(authenticationResponse);
            }
            else
            {
                string errorMessage = string.Join("|", result.Errors.Select(e => e.Description));
                return Problem(errorMessage);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> PostLogin(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return Problem(errorMessage);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.PersonName, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(loginDTO.PersonName);
                if (user == null)
                {
                    return NoContent();
                }

                await _signInManager.SignInAsync(user, false);
                var authenticationResponse = _jwt.CreateJwtToken(user);
                return Ok(authenticationResponse);
            
            }
            else
            {
                return Problem("Invalid username or password.");
            }

        }

        [HttpGet("logout")]
        public async Task<IActionResult> GetLogout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticationResponse>> RefreshToken([FromBody] string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiration < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");

            // Generate new JWT
            var newJwt = _jwt.CreateJwtToken(user);

            // Optionally generate new refresh token
            user.RefreshToken = newJwt.RefreshToken;
            user.RefreshTokenExpiration = newJwt.RefreshTokenExpiration;
            await _userManager.UpdateAsync(user);

            return Ok(newJwt);
        }

    }
}

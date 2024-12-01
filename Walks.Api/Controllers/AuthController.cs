using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using Walks.Api.Model.DTOs;
using Walks.Api.Repos;

namespace Walks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }



        //Register - Post method.
        //  /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            //user manager class gives us the 

            //get the data.
            var user = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            }; 

            //call the usermanager create method.
            var identityResult = await userManager.CreateAsync(user, registerRequestDto.Password);

            //check if the saving mechanism was successful.
            if (identityResult.Succeeded) {
                //Add roles to the user.
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    //assignment of roles.
                    identityResult = await userManager.AddToRolesAsync(user, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("Registration successful"); 
                    }
                }
            }

            //return bad request.
            return BadRequest(identityResult.Errors); 
        }

        //Login functionality.
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            //Find the email address through usermanager.
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            //check the validity.
            if (user ==null) {
                return BadRequest("User not found"); 
            }

            //check for password.
            var validUser = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (validUser)
            {

                //generate token.

                //Get roles.
                var roles = await userManager.GetRolesAsync(user);

                if (roles != null)
                {
                    //Generate token.
                    var token = tokenRepository.CreateToken(user, roles.ToList());

                    //convert it back to the DTO.
                    var response = new LoginResponseDto
                    {
                        JwtToken = token
                    }; 
                    return Ok(response);
                }
                return BadRequest("Token not generated"); 
            }


            //return bad request.
            return BadRequest("Username or password provided are incorrect"); 
        }

    }
}

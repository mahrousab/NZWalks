using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
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

		// post Register Method 
		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody]RegisterRequestDto registerRequestDto)
		{
			var identityUser = new IdentityUser
			{
				UserName = registerRequestDto.UserName,
				Email = registerRequestDto.UserName

			};

			var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

			if(identityResult.Succeeded)
			{
				if(registerRequestDto.Roles!= null && registerRequestDto.Roles.Any())
				{
					identityResult = await userManager.AddToRoleAsync(identityUser, registerRequestDto.Roles.ToString());

					if(identityResult.Succeeded)
					{
						return Ok("User Was Register");
					}
					
				}


			}
			return BadRequest("SomeThing Want Wrong");
		}


		[HttpPost]
		[Route("Login")]

		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
		{
			var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);

			if(user != null)
			{
				var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);


				if (checkPasswordResult)
				{

					// Get Roles for this users 

					var roles = await userManager.GetRolesAsync(user);


					if (roles != null)
					{
						// create Token 

						var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
						var response = new LoginResponseDto
						{
							JwtToken = jwtToken
						};
						return Ok(response);
					}


					
				}

			}

			return BadRequest("User Name Or Password incorrect");
		}
    }
}

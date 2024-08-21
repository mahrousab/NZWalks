using Microsoft.AspNetCore.Identity;

namespace NZWalks.Api.Repositories
{
	public interface ITokenRepository
	{
		string CreateJWTToken(IdentityUser user, List<string> roles);
	}
}

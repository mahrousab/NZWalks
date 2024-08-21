using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
	public interface IImageRepository
	{
		Task<Image> Upload(Image image);
	}
}

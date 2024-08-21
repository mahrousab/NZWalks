using NZWalks.Api.Data;
using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
	public class ImageRepository : IImageRepository
	{
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly NZDbContext context;

		public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZDbContext context)
        {
			this.webHostEnvironment = webHostEnvironment;
			this.httpContextAccessor = httpContextAccessor;
			this.context = context;
		}
        public async Task<Image> Upload(Image image)
		{
			var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "images",
				image.FileName, image.FileExtension);

			// Upload Image To Local Path

			using var stream = new FileStream(localFilePath, FileMode.Create);
			await image.File.CopyToAsync(stream);

			var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}: //{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

			image.FilePath = urlFilePath;

			// Save Image To Data Base 
			await context.Images.AddAsync(image);
			await context.SaveChangesAsync();

			return image;
			

		}
	}
}

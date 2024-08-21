using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Models.Domains;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IImageRepository imageRepository;

		public ImagesController(IImageRepository imageRepository)
        {
			this.imageRepository = imageRepository;
		}

        // post 

        [HttpPost]
		[Route("Upload")]
		public async Task<IActionResult> Upload(ImageUploadRequestDto request)
		{
			ValidateFileUpload(request);

			if (ModelState.IsValid)
			{
				// Convert DTO to Domain Model 

				var imageDomainModel = new Image
				{
					File = request.File,
					FileDescription = request.FileDescription,
					FileExtension = Path.GetExtension(request.File.FileName),
					FileName = request.FileName,
					FileSizeInBytes = request.File.Length
				};

				// Use Repository To Upload Image
				await imageRepository.Upload(imageDomainModel);

				return Ok(imageDomainModel);

			}

			return BadRequest(ModelState);
		}

		private void ValidateFileUpload(ImageUploadRequestDto request)
		{
			var allowExtensions = new string[] {".jpg",".jpeg",".png" };

			if(!allowExtensions.Contains(Path.GetExtension(request.FileName)))
			{
				ModelState.AddModelError("file", "UnSupportedFileExtensions");
			}
			if(request.File.Length > 10485670)
			{
				ModelState.AddModelError("file", "File size more 10mp");
			}
		}
	}
}

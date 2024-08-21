using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.Api.Models.DTO
{
	public class AddRegionRequestDto
	{
		[Required]
		[MinLength(3,ErrorMessage ="Code has to be a 3 minimum of characters")]
		[MaxLength(100, ErrorMessage ="Code has to be a 100 maximum of characters")]
		public string Code { get; set; }

		[Required]
		[MaxLength(100, ErrorMessage = "Name has to be a 100 maximum of characters")]
		public string Name { get; set; }
		public string? RegionImageUrl { get; set; }
	}
}

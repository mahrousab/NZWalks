using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Data;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Models.Domains;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Repositories;
using AutoMapper;
using NZWalks.Api.CustomActionFilters;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace NZWalks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegionsController : ControllerBase
	{

		private readonly IRegionRepository regionRepository;
		private readonly IMapper mapper;
		private readonly ILogger<RegionsController> logger;

		public RegionsController(IRegionRepository regionRepository, IMapper mapper,ILogger<RegionsController> logger)
		{

			this.regionRepository = regionRepository;
			this.mapper = mapper;
			this.logger = logger;
		}
		[HttpGet]
		//[Authorize(Roles ="Reader")]
		public async Task<IActionResult> GetAll()
		{

			try
			{
				throw new Exception("This is custom Exception");

				var regionsDomain = await regionRepository.GetAllAsync();

				logger.LogInformation($"Finished Get All Regions request with dat {JsonSerializer.Serialize(regionsDomain)}");

				return Ok (mapper.Map<List<RegionDto>>(regionsDomain));


			
			}
			catch(Exception ex)
			{
				logger.LogError(ex, ex.Message);
				throw;
			}
			
			

			

			
		}

		[HttpGet]
		[Route("{id:Guid}")]
		//[Authorize(Roles ="Reader")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var regionDomain = await regionRepository.GetById(id);
			if (regionDomain == null)
			{
				return NotFound();
			}

			// Map Region Domain To Region Dto 
			/*var regionsDto = new RegionDto
			{
				Id = regionDomain.Id,
				Code = regionDomain.Code,
				Name = regionDomain.Name,
				RegionImageUrl = regionDomain.RegionImageUrl
			};
			*/

			return Ok(mapper.Map<RegionDto>(regionDomain));
		}

		[HttpPost]
		[ValidateModel]
		//[Authorize(Roles ="Writer")]
		public async Task<IActionResult> Create(AddRegionRequestDto addRegionRequestDto)
		{

			// first map DTO to Domain Model  

			var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
			// use Domain Model To Create Region 

			regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);


			// Map Domain Model Back To DTO 
			var regionDto = mapper.Map<RegionDto>(regionDomainModel);

			return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
		}


		

		[HttpPut]
		[Route("{id:Guid}")]
		[ValidateModel]
		//[Authorize(Roles = "Writer")]
		public async Task <IActionResult> Update([FromRoute] Guid id, UpdateRegionRequestDto updateRegionRequestDto)
		{

			
				// map Dto To DomainModel 

				var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
				// Check if region exists 

				regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
				if (regionDomainModel == null)
				{
					return NotFound();
				}


				// convert Domain Model To DTO 

				var regionDto = mapper.Map<RegionDto>(regionDomainModel);
				return Ok(regionDto);
			
			
		}

		[HttpDelete]
		[Route("{id:Guid}")]
		//[Authorize(Roles = "Writer")]
		public async Task <IActionResult> Delete([FromRoute] Guid id)
		{

			var regionDomainModel = await regionRepository.DeleteAsync(id);
			if (regionDomainModel == null)
			{

				return NotFound();
			}

			 
			// map DomainModel To DTO

			
			return Ok(mapper.Map<RegionDto>(regionDomainModel));
		}
	}
}

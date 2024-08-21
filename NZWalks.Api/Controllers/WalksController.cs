using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.CustomActionFilters;
using NZWalks.Api.Models.Domains;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;
using System.Net;

namespace NZWalks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WalksController : ControllerBase
	{
		private readonly IMapper mapper;
		private readonly IWalkRepository walkRepository;

		public WalksController(IMapper mapper, IWalkRepository walkRepository)
		{
			this.mapper = mapper;
			this.walkRepository = walkRepository;
		}

		[HttpPost]
		[ValidateModel]
		public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
		{

			// Map Dto To DomainModel 
			var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

			await walkRepository.CreateAsync(walkDomainModel);

			// Map DomainModel to Dto 
			return Ok(mapper.Map<WalkDto>(walkDomainModel));
		}



	

		

		[HttpGet]

		public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
			[FromQuery] string? sortBy, [FromQuery] bool? isAscending,
			[FromQuery] int pageNumber=1, [FromQuery] int pageSize=1000)
		{
			
				var walkDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

			throw new Exception("this is a new Exception");

			     
				return Ok(mapper.Map<List<WalkDto>>(walkDomainModel));
			
		}

		[HttpGet]
		[Route("{id:Guid}")]

		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var walkDomainModel = await walkRepository.GetByIdAsync(id);

			if (walkDomainModel == null)
			{
				return NotFound();
			}
			return Ok(mapper.Map<WalkDto>(walkDomainModel));
		}

		[HttpPut]
		[Route("{id:Guid}")]
		[ValidateModel]

		public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
		{

			
				//  map Dto To Domain Model 

				var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

				walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

				if (walkDomainModel == null)
				{
					return NotFound();
				}
				// map Domain Model To Dto 

				return Ok(mapper.Map<WalkDto>(walkDomainModel));

			}

		[HttpDelete]
		[Route("{id:Guid}")]

		public async Task<IActionResult> Delete(Guid id)
		{
			var deleteWalkDomainModel = await walkRepository.DeleteAsync(id);

			if(deleteWalkDomainModel == null)
			{
				return NotFound();
			}

			//map Domain Model To DTO 

			return Ok(mapper.Map<WalkDto>(deleteWalkDomainModel));

		}
	}
}

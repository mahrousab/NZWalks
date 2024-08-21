using AutoMapper;
using NZWalks.Api.Models.Domains;
using NZWalks.Api.Models.DTO;

namespace NZWalks.Api.Mapping
{
	public class AutoMapperProfiles : Profile
	{
        public AutoMapperProfiles()
        {
			CreateMap<Region, RegionDto>().ReverseMap();
			CreateMap<AddRegionRequestDto, Region>().ReverseMap();
			CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

			// for WalkController

			CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
			CreateMap<Walk, WalkDto>().ReverseMap();
			CreateMap<Walk, UpdateWalkRequestDto>().ReverseMap();
			


		}
    }
}

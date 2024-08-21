using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
	public class RegionRepository : IRegionRepository
	{
		private readonly NZDbContext context;

		public RegionRepository(NZDbContext context)
        {
			this.context = context;
		}

		public async Task<Region> CreateAsync(Region region)
		{
			await context.Regions.AddAsync(region);
			await context.SaveChangesAsync();

			return region;
		}

		public async Task<Region> DeleteAsync(Guid id)
		{
			var region = await context.Regions.FirstOrDefaultAsync(x=>x.Id== id);

			if(region == null)
			{
				return null;
			}
			 context.Remove(region);
			await context.SaveChangesAsync();

			return region;
			
		}

		public async Task<List<Region>> GetAllAsync()
		{
		  return await context.Regions.ToListAsync();
		}

		public async Task<Region?> GetById(Guid id)
		{
			return await context.Regions.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Region> UpdateAsync(Guid id, Region region)
		{
			var regions = await context.Regions.FirstOrDefaultAsync(x => x.Id == id);

			if(regions == null)
			{
				return null;
			}
			regions.Code = region.Code;
			regions.Name = region.Name;
			regions.RegionImageUrl=region.RegionImageUrl;

			await context.SaveChangesAsync();
			return regions;
		}
	}
}

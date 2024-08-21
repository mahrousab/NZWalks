using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domains;

namespace NZWalks.Api.Repositories
{
	public class WalkRepository : IWalkRepository
	{
		private readonly NZDbContext context;

		public WalkRepository(NZDbContext context)
        {
			this.context = context;
		}

		public async Task<Walk> CreateAsync(Walk walk)
		{
			 await context.Walks.AddAsync(walk);

			await context.SaveChangesAsync();
			return walk;
		}

		public async Task<Walk?> DeleteAsync(Guid id)
		{
			var existingWalk = await context.Walks.FirstOrDefaultAsync(x => x.Id == id);

			if (existingWalk == null)
			{
				return null;
			}
			context.Walks.Remove(existingWalk);

			await context.SaveChangesAsync();
			return existingWalk;

		}

		public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
			string? sortBy = null, bool isAscending = true,
			int pageNumber = 1, int pageSize = 1000)
		{
		  var walks = context.Walks.Include("Difficulty").Include("Region").AsQueryable();

			// Filtering 
			if(string.IsNullOrWhiteSpace(filterOn)== false && string.IsNullOrWhiteSpace(filterQuery)== false)
			{
				if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					walks = walks.Where(x=>x.Name.Contains(filterQuery));
				}

				
			}

			// Sorting 

			if(string.IsNullOrWhiteSpace(sortBy) == false)
			{
				if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					walks = isAscending? walks.OrderBy(x=>x.Name) : walks.OrderByDescending(x=>x.Name);
				}
				else if(sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase)){
					walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
				}
			}

			// pagination

			var skipResults = (pageNumber - 1) * pageSize;
			return await walks.Skip(skipResults).Take(pageSize).ToListAsync();

			// return await context.Walks.Include("Difficulty").Include("Region").ToListAsync();
		}

		public async Task<Walk?> GetByIdAsync(Guid id)
		{
			return await context.Walks.Include("Difficulty").Include("Region")
				.FirstOrDefaultAsync(x=>x.Id == id);
		}

		public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
		{
			var existingWalk = await context.Walks.FirstOrDefaultAsync(x => x.Id == id);

			if(existingWalk == null)
			{
				return null;
			}
			existingWalk.Name = walk.Name;
			existingWalk.Description = walk.Description;
			existingWalk.LengthInKm = walk.LengthInKm;
			existingWalk.WalkImageUrl = walk.WalkImageUrl;
			existingWalk.DifficultyId = walk.DifficultyId;
			existingWalk.RegionId = walk.RegionId;

			await context.SaveChangesAsync(); 

			return existingWalk;

		}
	}
}

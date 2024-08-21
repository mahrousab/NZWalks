using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.Api.Data
{
	public class NZWalksAuthDbContext : IdentityDbContext
	{
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var readerRoleId = "73274a8e-6bef-403f-9a73-96a1cc38e260";

			var writeRoleId = "c902513f-c859-44cc-abaa-874d28e50a64";

			var roles = new List<IdentityRole>
			{
				new IdentityRole
				{
					Id = readerRoleId,
					ConcurrencyStamp = readerRoleId,
					Name= "Reader",
					NormalizedName = "Reader".ToUpper()
				},
				new IdentityRole
				{
					Id = writeRoleId,
					ConcurrencyStamp = writeRoleId,
					Name= "Writer",
					NormalizedName = "Writer".ToUpper()
				}
			};
			builder.Entity<IdentityRole>().HasData(roles);
		}
	}
}

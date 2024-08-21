using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Mapping;
using NZWalks.Api.Repositories;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.OpenApi.Models;
using System.Net.NetworkInformation;
using Microsoft.Extensions.FileProviders;
using Serilog;
using NZWalks.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var logger = new LoggerConfiguration().
	WriteTo.Console()
	.WriteTo.File("Logs/NzWalks_Log.text",rollingInterval : RollingInterval.Minute)
	.MinimumLevel.Warning()
	.CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

var ConnectionString = builder.Configuration.GetConnectionString("DefualtConnection") ??
	throw new InvalidOperationException("now connection string was found");
builder.Services.AddDbContext<NZDbContext>(options =>
options.UseSqlServer(ConnectionString));

builder.Services.AddDbContext<NZWalksAuthDbContext>(
options => options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

builder.Services.AddScoped<IRegionRepository, RegionRepository>();

builder.Services.AddScoped<IWalkRepository, WalkRepository>();

builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddIdentityCore<IdentityUser>()
	.AddRoles<IdentityRole>()
	.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
	.AddEntityFrameworkStores<NZWalksAuthDbContext>().
	AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add Authorization To Swagger
builder.Services.AddSwaggerGen(/*options =>
{
	options.SwaggerDoc("V1", new OpenApiInfo { Title = "NZ Walks API", Version = "V1" });
	options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
	{
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = JwtBearerDefaults.AuthenticationScheme
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = JwtBearerDefaults.AuthenticationScheme
				},
				Scheme = "Oauth2",
				Name = JwtBearerDefaults.AuthenticationScheme,
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});

}*/);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
	AddJwtBearer(options =>
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer= builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


/*
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "image")),
	RequestPath = "/Images"
});
*/
app.MapControllers();

app.Run();

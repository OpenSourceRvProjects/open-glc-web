using OpenGLC.MVC;
using OpenGLC.Models.Security;
using Microsoft.EntityFrameworkCore;
using OpenGLC.Data.Entities;
using AspNetCoreRateLimit;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.InjectServices();

builder.Services.AddDbContext<OpenglclevelContext>(options => options.
	   UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMvc()
		.AddSessionStateTempDataProvider();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

var securityKeys = builder.Configuration.GetSection("security").Get<SecurityKeys>();

builder.Services.AddSingleton<ISecurityKeys>((serviceProvider) =>
{
	return securityKeys;
});

builder.Services.AddCustomAuthentication(securityKeys.JWT_PrivateKey);

builder.Services.AddCustomSwaggerGen();

// Add IP Rate Limiting services without appsettings.json
builder.Services.AddInMemoryRateLimiting(); // Using in-memory store

builder.Services.Configure<IpRateLimitOptions>(options =>
{
	// Configure rate-limiting options directly here
	options.EnableEndpointRateLimiting = true;
	options.StackBlockedRequests = false;
	options.QuotaExceededResponse = new QuotaExceededResponse()
	{
		StatusCode = StatusCodes.Status429TooManyRequests,
		Content = "You had made so much calls to my api :c",
		
	};

	// General rate limit rules (example: 100 requests per minute and 500 requests per 5 minutes)
	options.GeneralRules = new List<RateLimitRule>
	{
		new RateLimitRule
		{
			Endpoint = "*", // Apply to all endpoints
            Period = "15s",   // 1 minute
            Limit = 100     // Limit to 100 requests per minute
        },
		new RateLimitRule
		{
			Endpoint = "GET:/api/Account/getStatus", // Apply to all endpoints
            Period = "15s",   // 1 minute
            Limit = 3     // Limit to 100 requests per minute
        },

	};
});

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Add Rate Limit Middleware
app.UseIpRateLimiting(); // Apply the rate limiting middleware

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

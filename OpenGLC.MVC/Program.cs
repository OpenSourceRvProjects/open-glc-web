using OpenGLC.MVC;
using OpenGLC.MVC.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddMvc()
		.AddSessionStateTempDataProvider();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();


////Google search: services AddScopped instanciate
////Initialize the Instances within ConfigServices in Startup .NET
////https://www.thecodebuzz.com/initialize-instances-within-configservices-in-startup/
////https://www.roundthecode.com/dotnet/how-to-read-the-appsettings-json-configuration-file-in-asp-net-core
var securityKeys = builder.Configuration.GetSection("security").Get<ISecurityKeys>();

builder.Services.AddSingleton<ISecurityKeys>((serviceProvider) =>
{
	return securityKeys;
});

builder.Services.AddCustomAuthentication(securityKeys.JWT_PrivateKey);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

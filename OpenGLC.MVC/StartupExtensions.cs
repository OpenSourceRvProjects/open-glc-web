using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenGLC.Backend.Services;
using OpenGLC.Infrastructure;
using OpenGLC.Infrastructure.Interfaces;
using OpenGLC.Infrastructure.Services;
using OpenGLC.Security;
using System.Text;


namespace OpenGLC.MVC
{
	public static class StartupExtensions
	{
		public static void AddCustomAuthentication(this IServiceCollection services, string JWTKey)
		{
			var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTKey));
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					RequireExpirationTime = true,
					ValidIssuer = "openglclevel-app",
					ValidAudience = "openglclevel-app",
					IssuerSigningKey = mySecurityKey,
					//https://stackoverflow.com/questions/43045035/jwt-token-authentication-expired-tokens-still-working-net-core-web-api
					ClockSkew = TimeSpan.Zero
				};
			});
		}

		public static void AddCustomSwaggerGen(this IServiceCollection services)
		{
			services.AddSwaggerGen(option =>
			{
				option.SwaggerDoc("v1", new OpenApiInfo { Title = "Openclg API", Version = "v1" });
				option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});
				option.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							}
						},
						new string[]{}
					}
				});
			});

		}

		public static void InjectServices(this IServiceCollection services)
		{
			services.AddTransient<IUserService, UserService>();
			//services.AddTransient<IMealItemService, MealItemService>();
			//services.AddTransient<IMealEventService, MealEventService>();

			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IMealItemRepository, MealItemRepository>();
			services.AddTransient<IMealEventRepository, MealEventRepository>();
			services.AddTransient<IMealEventItemsRepository, MealEventItemsRepository>();

			services.AddTransient<EncryptorEngine>();
			services.AddTransient<DecryptorEngine>();
			services.AddTransient<TokenHandlerEngine>();
			//services.AddTransient<ControllerUtilities>();
		}
	}

}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
	}
}

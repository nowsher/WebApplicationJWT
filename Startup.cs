using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApplicationJWT
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			/////read data from token.////////
			//var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QxIiwiZW1haWwiOiJhc2RAYXNkLmFkcyIsIm5iZiI6MTY3Mjk0OTk4OCwiZXhwIjoxNjcyOTUzNTg4LCJpYXQiOjE2NzI5NDk5ODh9.O4zshP-DYc_HozS16dSIeOeLqkZroKdpc4klg_F_U_0";// "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QxIiwibmJmIjoxNjcyOTQyODI3LCJleHAiOjE2NzI5NDY0MDIsImlhdCI6MTY3Mjk0MjgyN30.k2gh9ri4bN_pNs9pI68CIVKDyIYhNW24whY8ALklNVg";
			//var handler = new JwtSecurityTokenHandler();
			//var jwtSecurityToken = handler.ReadJwtToken(token);

			//var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QxIiwiZW1haWwiOiJhc2RAYXNkLmFkcyIsIm5iZiI6MTY3Mjk0OTk4OCwiZXhwIjoxNjcyOTUzNTg4LCJpYXQiOjE2NzI5NDk5ODh9.O4zshP-DYc_HozS16dSIeOeLqkZroKdpc4klg_F_U_0";// "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QxIiwibmJmIjoxNjcyOTQyODI3LCJleHAiOjE2NzI5NDY0MDIsImlhdCI6MTY3Mjk0MjgyN30.k2gh9ri4bN_pNs9pI68CIVKDyIYhNW24whY8ALklNVg";
			//var handler = new JwtSecurityTokenHandler();
			//var jsonToken = handler.ReadToken(token);
			//var tokenS = jsonToken as JwtSecurityToken;
			//var jti = tokenS.Claims.First(claim => claim.Type == "jti").Value;
			///////////////////////////////////

			//read from config file 'appsettings.json'
			var tokenKey = Configuration.GetValue<string>("JWTTokenKey");
			var key = tokenKey; // Encoding.ASCII.GetBytes(tokenKey);

			//var key = "this is my test key";


			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});

			services.AddSingleton<IJWTAuthenticationManager>(new JWTAuthenticationManager(key));//tokenKey
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();// must allow to authenticate. else no access to [Authorize] methods.

			app.UseAuthorization();// if blocked this line shows exception, NameController.Get (WebApplicationJWT) contains authorization metadata "[Authorize]", but a middleware was not found that supports authorization.

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}

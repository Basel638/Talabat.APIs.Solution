using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services


			//ApplicationServicesExtension.AddApplicationServices(webApplicationBuilder, webApplicationBuilder.Services);
			webApplicationBuilder.Services.AddApplicationServices(webApplicationBuilder);
			#endregion

			var app = webApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();

			var services= scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>();

			var _IdentitydbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
			// Ask CLR for Creating Object from DbContext Explicitly

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{

				await _dbContext.Database.MigrateAsync();   // Update-Database

				await StoreContextSeed.SeedAsync(_dbContext); // Data Seeding

				await _IdentitydbContext.Database.MigrateAsync();// Update-Database

				var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				
				await ApplicationIdentityContextSeed.SeedUsersAsync(_userManager);
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "an error has been occured during apply the migrations");
			}

			#region Configure Kestrel Middlewares

			app.UseMiddleware<ExceptionMiddleware>();
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{

				app.UseSwagger();
				app.UseSwaggerUI();


			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseCors("MyPolicy");

			app.UseStaticFiles();

			app.MapControllers();

			#endregion

			 
			app.Run(); 
		}
	}
}

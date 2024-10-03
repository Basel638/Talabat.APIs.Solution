using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the DI container.

			webApplicationBuilder.Services.AddControllers();
			// Register Required Web APIs Services to the DI Container



			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen(); 



			webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			
			});

			webApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			//webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
			webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));


			webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options => {
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
														 .SelectMany(P => P.Value.Errors)
														 .Select(E => E.ErrorMessage)
														 .ToList();
					var response = new ApiValidationErrorResponse()
					{
						Errors= errors
					};

					return new BadRequestObjectResult(response);
				};
			
			});
			#endregion

			var app = webApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();

			var services= scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>();
			// Ask CLR for Creating Object from DbContext Explicitly

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{

				await _dbContext.Database.MigrateAsync();   // Update-Database

				await StoreContextSeed.SeedAsync(_dbContext); // Data Seeding
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

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.MapControllers();

			#endregion

			 
			app.Run(); 
		}
	}
}

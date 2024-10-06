using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;
using Talabat.Repository;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
		{
			// Add services to the DI container.

			services.AddControllers();
			// Register Required Web APIs Services to the DI Container



			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();



			services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));

			});

			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			//services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
			services.AddAutoMapper(typeof(MappingProfiles));


			services.Configure<ApiBehaviorOptions>(options => {
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
														 .SelectMany(P => P.Value.Errors)
														 .Select(E => E.ErrorMessage)
														 .ToList();
					var response = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(response);
				};

			});
			return services;
		}
	}
}

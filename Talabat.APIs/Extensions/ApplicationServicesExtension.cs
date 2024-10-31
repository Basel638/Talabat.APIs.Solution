using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;
using Talabat.Repository;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using StackExchange.Redis;
using Talabat.Repository.Identity;
using Talabat.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Service.AuthService;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Talabat.Core;
using Talabat.Service.OrderService;
using Talabat.Service.ProductService;
using Talabat.Service.PaymentService;
using Talabat.Service.CacheService;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
		{

			services.AddCors( options =>
			{
				options.AddPolicy("MyPolicy", policyOptions =>
				{
					policyOptions.AllowAnyHeader().AllowAnyMethod().WithOrigins(webApplicationBuilder.Configuration["FrontBaseUrl"]);
				});
			});

			services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));

			services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

			services.AddScoped(typeof(IOrderService), typeof(OrderService));

			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

			services.AddScoped(typeof(IProductService), typeof(ProductService));
			//services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



			// Add services to the DI container.
			services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});
			// Register Required Web APIs Services to the DI Container



			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();



			services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));

			});


			services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);

			});


			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));


			//services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
			services.AddAutoMapper(typeof(MappingProfiles));

			services.AddScoped(typeof(IAuthService), typeof(AuthService));

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


			services.AddDbContext<ApplicationIdentityDbContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
			});


			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{


			}).AddEntityFrameworkStores<ApplicationIdentityDbContext>(); ;


			#region Add AuthServices
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = webApplicationBuilder.Configuration["JWT:ValidIssuer"],
					ValidateAudience = true,
					ValidAudience = webApplicationBuilder.Configuration["JWT:ValidAudience"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationBuilder.Configuration["JWT:AuthKey"] ?? string.Empty)),
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,

				};
			}); 
			#endregion





			return services;
		}
	}
}


using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddleWares
{
	// By Convension
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
			_next = next;
			_logger = logger;
			_env = env;
		}
        public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				// Take an Aciton with the Request

				await _next.Invoke(httpContext); // Go To The Next Middleware

				// Take an action with the Response
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message); // Development Env
											  // Log Exception in (Database | Files) // Production Env


				httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

				httpContext.Response.ContentType = "application/json";

				var response = _env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
					: new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

				var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

				var json = JsonSerializer.Serialize(response, options);


				await httpContext.Response.WriteAsync(json);
			}
		}
	}
}

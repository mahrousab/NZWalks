using System.Net;

namespace NZWalks.Api.Middlewares
{
	public class ExceptionHandlerMiddleware
	{
		private readonly ILogger<ExceptionHandlerMiddleware> logger;
		private readonly RequestDelegate next;

		public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
			this.logger = logger;
			this.next = next;
		}


		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await next(httpContext);
			}catch (Exception ex)
			{
				var errorId = Guid.NewGuid();

				logger.LogError(ex,$"{errorId}: {ex.Message}");

				// Return A Custom Error Response


				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				httpContext.Response.ContentType = "application/json";


				var error = new
				{
					Id = errorId,
					ErrorMessage = "Something want wrong we will resolved it"

				};
				await httpContext.Response.WriteAsJsonAsync(error);
			}
		}
    }
}

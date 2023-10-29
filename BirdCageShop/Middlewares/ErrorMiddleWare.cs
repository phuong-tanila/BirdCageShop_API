using Microsoft.AspNetCore.Mvc;
using Repositories.Commons.Exceptions;
using System.Text.Json;

namespace BirdCageShop.Middlewares
{
    public class ErrorMiddleWare
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseBussinessLogicException ex)
            {
                var response = context.Response;
                response.StatusCode = ex.StatusCode;
                ProblemDetails problem = new ProblemDetails()
                {
                    Type = "Business logic exception",
                    Status = ex.StatusCode,
                    Detail = JsonSerializer.Serialize(ex.ErrorMessages)
                };
                response.WriteAsJsonAsync(problem);
            }
        }
    }
}

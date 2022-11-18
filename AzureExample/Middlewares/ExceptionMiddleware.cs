using AzureExample.Configurations;
using AzureExample.Exceptions;
using AzureExample.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace AzureExample.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ApplicationSettings applicationSettings;

        public ExceptionMiddleware(RequestDelegate next, IOptions<ApplicationSettings> options)
        {
            this.next = next;
            this.applicationSettings = options.Value;
        }

        public async Task Invoke(HttpContext context) 
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorResponse = new ErrorResponseModel
            {
                Title = "Internal Server Error",
                Description = "Something went wrong."
            };

            switch (exception)
            {
                case BlobException blobException:
                    if (blobException.Title != null)
                    { 
                        errorResponse.Title = blobException.Title;
                    }
                    if (blobException.Message != null)
                    { 
                        errorResponse.Description = blobException.Message;
                    }
                    break;
            }

            if (applicationSettings.DebugMode)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
            else
            {
                await context.Response.WriteAsync(exception.ToString());
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}

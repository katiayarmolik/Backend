using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BlogApi.Data;
using PvHttpRouter.Abstractions;
using PvHttpRouter.Attributes;

namespace BlogApi.Handlers
{
    [Route("get")]
    public class GetHandler : IRouteHandler
    {
        public async Task HandleAsync(HttpListenerContext context)
        {
            string? type = context.Request.QueryString["type"];

            string responseJson;

            if (type == "users")
            {
                responseJson = JsonSerializer.Serialize(DataWrapper.Users, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            else if (type == "comments")
            {
                responseJson = JsonSerializer.Serialize(DataWrapper.Comments, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            else
            {
                var errorResponse = new { error = "Invalid type" };
                responseJson = JsonSerializer.Serialize(errorResponse);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; 
                await WriteResponseAsync(context, responseJson);
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await WriteResponseAsync(context, responseJson);
        }

        private async Task WriteResponseAsync(HttpListenerContext context, string responseContent)
        {
            using (var writer = new StreamWriter(context.Response.OutputStream))
            {
                await writer.WriteAsync(responseContent);
            }
        }
    }
}

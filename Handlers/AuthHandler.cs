using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BlogApi.Data;
using PvHttpRouter.Abstractions;
using PvHttpRouter.Attributes;

namespace BlogApi.Handlers
{
    [Route("auth")]
    public class AuthHandler : IRouteHandler
    {
        public async Task HandleAsync(HttpListenerContext context)
        {
            using var reader = new StreamReader(context.Request.InputStream);
            var body = await reader.ReadToEndAsync();
            var user = JsonSerializer.Deserialize<Entities.User>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Console.WriteLine("AuthHandler: Body: " + body);

            if (user != null)
            {
                var foundUser = DataWrapper.FindUserByUsername(user.Username);
                Console.WriteLine("Знайдений юзер: " + (foundUser?.Username ?? "null"));

                if (foundUser != null && foundUser.Password == user.Password)
                {
                    var responseJson = JsonSerializer.Serialize(new { Id = foundUser.Id });
                    context.Response.StatusCode = (int)HttpStatusCode.OK;

                    using (var writer = new StreamWriter(context.Response.OutputStream))
                    {
                        await writer.WriteAsync(responseJson);
                    }

                    Console.WriteLine("AuthHandler: Аунтефіковано");
                    return;
                }
            }

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var errorResponse = JsonSerializer.Serialize(new { Id = (Guid?)null });
            using (var writer = new StreamWriter(context.Response.OutputStream))
            {
                await writer.WriteAsync(errorResponse);
            }

            Console.WriteLine("AuthHandler: Помилка аунтефікації");
        }
    }
}

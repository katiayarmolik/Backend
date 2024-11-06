using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BlogApi.Data;
using BlogApi.Entities;
using PvHttpRouter.Abstractions;
using PvHttpRouter.Attributes;

namespace BlogApi.Handlers
{
    [Route("register")]
    public class RegisterHandler : IRouteHandler
    {
        public async Task HandleAsync(HttpListenerContext context)
        {
            Console.WriteLine("RegisterHandler: Запит отримано");

            try
            {
                using var reader = new StreamReader(context.Request.InputStream);
                var body = await reader.ReadToEndAsync();
                Console.WriteLine("RegisterHandler: Body: " + body);

                var user = JsonSerializer.Deserialize<User>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errorResponse = JsonSerializer.Serialize(new { error = "Invalid username or password" });

                    using var writer = new StreamWriter(context.Response.OutputStream);
                    await writer.WriteAsync(errorResponse);

                    Console.WriteLine("RegisterHandler: Поганий запит - Не вірний юзернейм або пароль");
                    return;
                }


                user.Id = Guid.NewGuid();
                DataWrapper.Users.Add(user);

                context.Response.StatusCode = (int)HttpStatusCode.Created;
                var successResponse = JsonSerializer.Serialize(new { Id = user.Id });

                using var successWriter = new StreamWriter(context.Response.OutputStream);
                await successWriter.WriteAsync(successResponse);

                Console.WriteLine("RegisterHandler: Користувача зареєстровано із ID: " + user.Id);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorResponse = JsonSerializer.Serialize(new { error = "An unexpected error occurred", details = ex.Message });

                using var writer = new StreamWriter(context.Response.OutputStream);
                await writer.WriteAsync(errorResponse);

                Console.WriteLine("RegisterHandler: помилка - " + ex.Message);
            }
        }
    }
}

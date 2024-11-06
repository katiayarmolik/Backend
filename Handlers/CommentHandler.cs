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
    [Route("comment")]
    public class CommentHandler : IRouteHandler
    {
        public async Task HandleAsync(HttpListenerContext context)
        {
            if (context.Request.Headers["Authentication"] is not string userIdStr || !Guid.TryParse(userIdStr, out var userId))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var errorResponse = JsonSerializer.Serialize(new { error = "Unauthorized: Invalid or missing user ID" });
                await WriteResponseAsync(context, errorResponse);
                return;
            }

            if (DataWrapper.FindUserById(userId) == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var errorResponse = JsonSerializer.Serialize(new { error = "Unauthorized: User not found" });
                await WriteResponseAsync(context, errorResponse);
                return;
            }

            using var reader = new StreamReader(context.Request.InputStream);
            var body = await reader.ReadToEndAsync();
            var commentData = JsonSerializer.Deserialize<Comment>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Console.WriteLine("commentData: " + commentData);

            if (commentData != null && !string.IsNullOrEmpty(commentData.Text))
            {
                var comment = new Comment
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Text = commentData.Text
                };
                DataWrapper.Comments.Add(comment);

                context.Response.StatusCode = (int)HttpStatusCode.Created;
                var responseJson = JsonSerializer.Serialize(comment);
                await WriteResponseAsync(context, responseJson);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errorResponse = JsonSerializer.Serialize(new { error = "Bad Request: Comment text is required" });
                await WriteResponseAsync(context, errorResponse);
            }
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

using BlogApi.Entities;
using PvHttpRouter;

var server = new HttpServerRouter("localhost", 5000);

Console.WriteLine("Starting server...");
await server.StartAsync();

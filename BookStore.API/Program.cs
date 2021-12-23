using BookStore.API;

var builder = CreateWebHost(args);
var app = builder.Build();

app.Run();

IHostBuilder CreateWebHost(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}
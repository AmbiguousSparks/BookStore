using BookStore.API;

var builder = CreateWebHost(args)
    .ConfigureAppConfiguration(c =>
    {
        c.AddJsonFile("appsettings.json", false, true);
        c.AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json",
            true);
        c.AddEnvironmentVariables();
    });

var app = builder.Build();

app.Run();

IHostBuilder CreateWebHost(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}
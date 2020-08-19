using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace WMSAuthentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Process);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var urls = configuration.GetValue("UseUrls", "http://0.0.0.0");

            Console.WriteLine($"Using ASPNETCORE_ENVIRONMENT = {environmentName}");

            CreateWebHostBuilder(args, urls).Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args, string urls) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls(urls)
                .UseStartup<Startup>()
                .Build();
    }
}

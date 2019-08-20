namespace OAuthServer
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "OAuthServer";

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5000")
                .UseStartup<Startup>();
    }
}

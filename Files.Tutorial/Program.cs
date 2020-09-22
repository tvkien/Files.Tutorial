using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Files.Tutorial
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        options.Limits.MaxRequestBodySize = int.MaxValue;
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
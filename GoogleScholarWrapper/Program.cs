using BibliographicalSourcesIntegratorContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GoogleScholarWrapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(new string[] { ProgramAddresses.GoogleScholarWrapperAddress });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
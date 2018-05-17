using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Alipay.Demo.PCPayment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (Action<WebHostBuilderContext, IConfigurationBuilder>) ((hostingContext, config) =>
                        {
                            config.AddJsonFile("alipay.json");
                        }))
                .UseStartup<Startup>()
                .Build();
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using listening.Security;

namespace listening
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();

            var certName = SecurityRulesSingleton.Instance.Rules.CertificateName;
            var password = SecurityRulesSingleton.Instance.Rules.Passwd;
            var pfxFile = Path.Combine(Directory.GetCurrentDirectory(), certName);
            var certificate = new X509Certificate2(pfxFile, password);

#if ASPNETCORE_ENVIRONMENT == Production
Console.WriteLine("Production aaa das ist fantastish");
#else
Console.WriteLine("Not Production");
#endif
            var host = new WebHostBuilder()
                .UseConfiguration(config)
                // #if ASPNETCORE_ENVIRONMENT == Production
                .UseKestrel(opt => opt.UseHttps(certificate))
                // #else
                .UseKestrel()
                // #endif
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}

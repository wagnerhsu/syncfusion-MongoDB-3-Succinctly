using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLogWrapper;

namespace ConsoleUpdateData
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            NLogServiceNoConfigFile.BuildLogingConfiguration(new NLogOptions());
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Info("Starting console host.");
                await CreateHostBuilder(args).RunConsoleAsync();
                return 0;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseAutofac()
                .ConfigureLogging(NLogServiceNoConfigFile.ConfigureLogging)
                .ConfigureAppConfiguration((context, config) =>
                {
                    //setup your additional configuration sources
                })
                .ConfigureServices((hostContext, services) => { services.AddApplication<ConsoleUpdateDataModule>(); });
        }
    }
}
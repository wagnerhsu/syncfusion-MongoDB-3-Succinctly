using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace ConsoleInsertData
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var config = new NLog.Config.LoggingConfiguration();

// Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "${basedir}/${processname}.log",
                Layout = "${longdate:format=yyyy-MM-dd HH:mm:ss.ffffff}|${logger}|${level:uppercase=true}|${processid}|${threadid}|${message} ${exception:format=tostring}"
            };
            var logconsole = new NLog.Targets.ColoredConsoleTarget("logconsole")
            {
                Layout = "${longdate:format=yyyy-MM-dd HH:mm:ss.ffffff}|${logger}|${level:uppercase=true}|${processid}|${threadid}|${message} ${exception:format=tostring}"
            };

// Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

// Apply config           
            LogManager.Configuration = config;
            var Log = LogManager.GetCurrentLogger();
            try
            {
                Log.Information("Starting console host.");
                await CreateHostBuilder(args).RunConsoleAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseAutofac()
                .UseNLog()
                .ConfigureAppConfiguration((context, config) =>
                {
                    //setup your additional configuration sources
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplication<ConsoleInsertDataModule>();
                });
    }
}

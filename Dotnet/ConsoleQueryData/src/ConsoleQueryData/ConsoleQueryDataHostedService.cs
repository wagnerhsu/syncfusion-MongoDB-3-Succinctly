using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace ConsoleQueryData
{
    public class ConsoleQueryDataHostedService : IHostedService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly IServiceProvider _serviceProvider;
        private readonly QueryBsonDoc _queryBsonDoc;

        public ConsoleQueryDataHostedService(
            IAbpApplicationWithExternalServiceProvider application,
            IServiceProvider serviceProvider,
            QueryBsonDoc queryBsonDoc
            )
        {
            _application = application;
            _serviceProvider = serviceProvider;
            _queryBsonDoc = queryBsonDoc;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _application.Initialize(_serviceProvider);
            await _queryBsonDoc.RunAsync();

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.Shutdown();

            return Task.CompletedTask;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace ConsoleDeleteData
{
    public class ConsoleDeleteDataHostedService : IHostedService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly IServiceProvider _serviceProvider;
        private readonly DeleteDataWithBsonMapper _deleteDataWithBsonMapper;

        public ConsoleDeleteDataHostedService(
            IAbpApplicationWithExternalServiceProvider application,
            IServiceProvider serviceProvider,
            DeleteDataWithBsonMapper deleteDataWithBsonMapper
            )
        {
            _application = application;
            _serviceProvider = serviceProvider;
            _deleteDataWithBsonMapper = deleteDataWithBsonMapper;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _application.Initialize(_serviceProvider);

            await _deleteDataWithBsonMapper.RunAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.Shutdown();

            return Task.CompletedTask;
        }
    }
}
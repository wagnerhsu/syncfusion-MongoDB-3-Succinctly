﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace ConsoleQueryData
{
    public class ConsoleQueryDataHostedService : IHostedService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly QueryBsonDoc _queryBsonDoc;
        private readonly QueryWithLinq _queryWithLinq;
        private readonly IServiceProvider _serviceProvider;

        public ConsoleQueryDataHostedService(
            IAbpApplicationWithExternalServiceProvider application,
            IServiceProvider serviceProvider,
            QueryBsonDoc queryBsonDoc, QueryWithLinq queryWithLinq
        )
        {
            _application = application;
            _serviceProvider = serviceProvider;
            _queryBsonDoc = queryBsonDoc;
            _queryWithLinq = queryWithLinq;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _application.Initialize(_serviceProvider);
            await _queryBsonDoc.RunAsync();
            await _queryWithLinq.RunAsync();
            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.Shutdown();

            return Task.CompletedTask;
        }
    }
}
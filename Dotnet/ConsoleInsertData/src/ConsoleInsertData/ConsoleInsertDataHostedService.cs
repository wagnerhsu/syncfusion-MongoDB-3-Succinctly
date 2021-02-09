﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace ConsoleInsertData
{
    public class ConsoleInsertDataHostedService : IHostedService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly InsertBsonDoc _insertBsonDoc;
        private readonly InsertMovieDb _insertMovieDb;
        private readonly IServiceProvider _serviceProvider;

        public ConsoleInsertDataHostedService(
            IAbpApplicationWithExternalServiceProvider application,
            IServiceProvider serviceProvider,
            InsertBsonDoc insertBsonDoc,
            InsertMovieDb insertMovieDb
        )
        {
            _application = application;
            _serviceProvider = serviceProvider;
            _insertBsonDoc = insertBsonDoc;
            _insertMovieDb = insertMovieDb;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _application.Initialize(_serviceProvider);
            await _insertBsonDoc.RunAsync();
            await _insertMovieDb.RunAsync();
            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.Shutdown();

            return Task.CompletedTask;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Week2_Homework2.Data.Context;

namespace Week2_Homework2.Workers
{
    public class MovieWorkerService : BackgroundService
    {

        private ILogger<MovieWorkerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private DatabaseContext _dbContext;

        public MovieWorkerService(ILogger<MovieWorkerService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var scope = _scopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                if(_dbContext == null)
                {
                    var scope = _scopeFactory.CreateScope();
                    _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                }

                var adminRecords = await _dbContext.User
                                                  .Where(user => !user.IsAdmin).ToListAsync();

                    foreach(var record in adminRecords)
                    {
                        record.IsAdmin = true;
                    }

                    if(_dbContext.ChangeTracker.HasChanges())
                        await _dbContext.SaveChangesAsync();

                    _logger.LogInformation("Worker Runing");
                

                await Task.Delay(3000, stoppingToken);


            }
        }
    }
}

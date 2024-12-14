using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Client.WorkerService
{
    public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private HubConnection? _hubConnection;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(configuration.GetSection("SignalR")["Hub"]!).Build();

            _hubConnection.StartAsync().ContinueWith((result) =>
            {
                _logger.LogInformation(result.IsCompletedSuccessfully ? "Connected" : "Connected Fail");
            });
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            #region Default
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    if (_logger.IsEnabled(LogLevel.Information))
            //    {
            //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    }
            //    await Task.Delay(1000, stoppingToken);
            //}
            #endregion

            _hubConnection.On<Product>("ReceiveTypedMessageForAllClient", (product) =>
            {
                _logger.LogInformation($"Received message: {product.Id}--{product.Name}--{product.Price}");
            });
            return Task.CompletedTask;

        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _hubConnection.StopAsync(cancellationToken);
            await _hubConnection.DisposeAsync();
            base.StopAsync(cancellationToken);
        }
    }
}

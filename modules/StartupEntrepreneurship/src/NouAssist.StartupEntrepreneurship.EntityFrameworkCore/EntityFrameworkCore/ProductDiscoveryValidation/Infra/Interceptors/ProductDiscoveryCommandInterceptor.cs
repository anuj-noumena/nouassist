namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Interceptors
{
    /// <summary>
    /// Command interceptor for Product Discovery operations
    /// </summary>
    public class ProductDiscoveryCommandInterceptor : DbCommandInterceptor
    {
        private readonly ILogger<ProductDiscoveryCommandInterceptor> _logger;

        public ProductDiscoveryCommandInterceptor(ILogger<ProductDiscoveryCommandInterceptor> logger)
        {
            _logger = logger;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            LogCommand(command, "Executing");
            return base.ReaderExecuting(command, eventData, result);
        }

        public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            LogCommand(command, "Executing Async");
            return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override DbDataReader ReaderExecuted(
            DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result)
        {
            LogCommand(command, $"Executed in {eventData.Duration.TotalMilliseconds}ms");
            return base.ReaderExecuted(command, eventData, result);
        }

        public override async ValueTask<DbDataReader> ReaderExecutedAsync(
            DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result,
            CancellationToken cancellationToken = default)
        {
            LogCommand(command, $"Executed Async in {eventData.Duration.TotalMilliseconds}ms");
            return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }

        private void LogCommand(DbCommand command, string status)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Product Discovery DB Command {Status}: {CommandText}", 
                    status, command.CommandText);
            }
        }
    }
}

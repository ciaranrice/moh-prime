using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prime.HttpClients;
using Prime.Models;
using Prime.Services;

namespace Prime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OdrController : PrimeControllerBase
    {
        private readonly IPrimeOdrClient _primeOdrClient;

        private readonly IPharmanetTransactionLogService _pnetTransactionLogService;

        private readonly ILogger _logger;


        public OdrController(
            IPrimeOdrClient primeOdrClient,
            IPharmanetTransactionLogService pnetTransactionLogService,
            ILogger<OdrController> logger)
        {
            _primeOdrClient = primeOdrClient;
            _pnetTransactionLogService = pnetTransactionLogService;
            _logger = logger;
        }


        /// <summary>
        /// Expose method invokable via OpenShift Cron
        /// </summary>
        [HttpPost("retrieve-logs", Name = nameof(RetrievePharmanetTxLogs))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RetrievePharmanetTxLogs()
        {
            // TODO: Add exception handling

            long lastKnownTxId = _pnetTransactionLogService.GetMostRecentTransactionId();
            List<PharmanetTransactionLog> logs;
            bool existsMore;
            do
            {
                (logs, existsMore) = await _primeOdrClient.RetrieveLatestPharmanetTxLogsAsync(lastKnownTxId);
                _logger.LogInformation($"{logs.Count} log items retrieved");
                _logger.LogInformation($"Do more logs exist?  {existsMore}");
                if (logs.Count > 0)
                {
                    try
                    {
                        await Task.Delay(int.Parse(PrimeEnvironment.PrimeOdrApi.SaveDelay));

                        lastKnownTxId = await _pnetTransactionLogService.SaveLogsAsync(logs);
                    }
                    catch (Npgsql.NpgsqlException e)
                    {
                        _logger.LogError(e.Message, e);
                        // TODO: Make configurable
                        // Wait for database to recover and then try again
                        await Task.Delay(5000);
                    }
                }
            } while (existsMore);

            return Ok(lastKnownTxId);
        }
    }
}

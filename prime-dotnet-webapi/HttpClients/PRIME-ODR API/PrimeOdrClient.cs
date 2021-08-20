using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prime.Models;
using Flurl;
using Newtonsoft.Json;

namespace Prime.HttpClients
{
    public class PrimeOdrClient : BaseClient, IPrimeOdrClient
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        public PrimeOdrClient(
            HttpClient client,
            ILogger<PrimeOdrClient> logger) : base(PropertySerialization.CamelCase)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<(List<PharmanetTransactionLog> Logs, bool ExistsMoreLogs)> RetrieveLatestPharmanetTxLogsAsync(long lastKnownTxId)
        {
            _logger.LogInformation(PrimeEnvironment.PrimeOdrApi.Url);
            _logger.LogInformation(PrimeEnvironment.PrimeOdrApi.ClientName);
            _logger.LogInformation(PrimeEnvironment.PrimeOdrApi.FetchSize);

            // Auth header and cert are configured to be injected in Startup.cs
            string primeRequestId = System.Guid.NewGuid().ToString();
            var httpResponse = await _client.GetAsync(PrimeEnvironment.PrimeOdrApi.Url.SetQueryParams(
                new
                {
                    requestUUID = primeRequestId,
                    clientName = PrimeEnvironment.PrimeOdrApi.ClientName,
                    lastTxnId = lastKnownTxId,
                    fetchSize = PrimeEnvironment.PrimeOdrApi.FetchSize
                }));

            //            var apiResponse = await httpResponse.Content.ReadAsAsync<PrimeOdrApiResponse>();
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                // TODO: handle properly
                _logger.LogError(content);
                return (null, false);
            }

            var apiResponse = JsonConvert.DeserializeObject<PrimeOdrApiResponse>(content);
            _logger.LogInformation(@"Sent request Id: {primeRequestId}, response request Id: {apiResponse.RequestUUID}");
            _logger.LogInformation(@"Requested fetch size: {PrimeEnvironment.PrimeOdrApi.FetchSize}, response numberOfTransactions: {apiResponse.NumberOfTransactions}, actual count of transactions: {apiResponse.PNetTransactions.Count}");

            return (apiResponse.PNetTransactions, apiResponse.IsThereMoreData == "Y");
        }
    }


    public class PrimeOdrClientHandler : HttpClientHandler
    {
        public PrimeOdrClientHandler(ILogger<PrimeOdrClientHandler> logger)
        {
            logger.LogInformation(PrimeEnvironment.PrimeOdrApi.SslCertFilename);
            logger.LogInformation(PrimeEnvironment.PrimeOdrApi.SslCertPassword);

            ClientCertificates.Add(new X509Certificate2(PrimeEnvironment.PrimeOdrApi.SslCertFilename, PrimeEnvironment.PrimeOdrApi.SslCertPassword));
            ClientCertificateOptions = ClientCertificateOption.Manual;
        }
    }

    public class PrimeOdrApiResponse
    {
        /// <summary>
        /// "UUID generated by client, returned in response, used to track request/response"
        /// </summary>
        public string RequestUUID { get; set; }

        /// <summary>
        /// Count of the number of PNet transaction records returned in the response, information only
        /// </summary>
        public int NumberOfTransactions { get; set; }

        /// <summary>
        /// "Boolean to indicate when all data is not returned and there are more transaction log records, Y/N value"
        /// </summary>
        public string IsThereMoreData { get; set; }

        // TODO: Interface type instead?
        public List<PharmanetTransactionLog> PNetTransactions { get; set; }
    }
}


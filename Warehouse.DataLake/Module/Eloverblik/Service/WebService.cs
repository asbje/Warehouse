using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Warehouse.DataLake.FunctionApp;

[assembly: InternalsVisibleTo("Warehouse.HEMS.Tests")]
namespace Warehouse.DataLake.Module.Eloverblik.Service
{
    public class WebService
    {
        private DateTime lastDownloadOfAccessToken;
        private HttpClient _client;
        private readonly string bearer;
        private readonly string baseUrl;
        public HttpResponseMessage ClientResponse { get; private set; }

        public HttpClient Client
        {
            get
            {
                if (_client == null || lastDownloadOfAccessToken.AddHours(1) < DateTime.Now)
                {
                    _client = GetHttpClient().Result;
                    lastDownloadOfAccessToken = DateTime.Now;
                }

                return _client;
            }
        }

        public WebService(string bearer, string baseUrl)
        {
            this.bearer = bearer;
            this.baseUrl = baseUrl;
        }

        /// <summary>
        /// Gets an access token that can be used for up to one hour, then it has to be revoked.
        /// </summary>
        internal async Task<HttpClient> GetHttpClient()
        {
            var handler = new HttpClientHandler();
            var client = new HttpClient(handler);
            client.Timeout = new TimeSpan(10, 0, 0);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            client.BaseAddress = new Uri(baseUrl);
            ClientResponse = await client.GetAsync("api/Token");
            if (ClientResponse.StatusCode != HttpStatusCode.OK)
                return null;

            var json = await ClientResponse.Content.ReadAsStringAsync();
            var token = JObject.Parse(json)["result"].ToString();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        /// <summary>
        /// This request is used for getting a list of metering points associated with a specific user (either private or business user).
        /// </summary>
        public async Task<HttpResponseMessage> GetMeteringPoints()
        {
            return Client != null ? await Client.GetAsync("api/MeteringPoints/MeteringPoints?includeAll=true") : ClientResponse;
        }

        /// <summary>
        /// This request is used for querying details (master data) for one or more (linked/related) metering point.
        /// </summary>
        public async Task<HttpResponseMessage> GetMeteringPointsDetails(string[] meteringPointIds)
        {
            if (Client == null) 
                return ClientResponse;

            if (meteringPointIds == null || !meteringPointIds.Any()) 
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Meteringpoint input is missing" };

            var json = new JObject(new JProperty("meteringPoints", new JObject(new JProperty("meteringPoint", new JArray(meteringPointIds)))));
            var data = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            return await Client.PostAsync("api/MeteringPoints/MeteringPoint/GetDetails", data);
        }

        /// <summary>
        /// This request is used for querying charge data(subscriptions, tariffs and fees) for one or more
        /// (linked/related) metering points.Charges linked to the metering point at the time of the request or on any future date will be returned.
        /// </summary>
        public async Task<HttpResponseMessage> GetMeteringPointsCharges(string[] meteringPointIds)
        {
            if (Client == null)
                return ClientResponse;

            if (meteringPointIds == null || !meteringPointIds.Any())
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Meteringpoint input is missing" };

            var json = new JObject(new JProperty("meteringPoints", new JObject(new JProperty("meteringPoint", new JArray(meteringPointIds)))));
            StringContent data = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            return await Client.PostAsync("api/MeteringPoints/MeteringPoint/GetCharges", data);
        }

        /// <summary>
        /// This request is used for querying time series for one or more (linked/related) metering points for a specified period and with a specified aggregation level.
        /// </summary>
        public async Task<HttpResponseMessage> GetMeterDataTimeSeries(string[] meteringPointIds, DateTime dateFrom, DateTime dateTo, TimeAggregation aggregation)
        {
            if (Client == null) 
                return ClientResponse;

            if (meteringPointIds == null || !meteringPointIds.Any())
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Meteringpoint input is missing" };

            var json = new JObject(new JProperty("meteringPoints", new JObject(new JProperty("meteringPoint", new JArray(meteringPointIds)))));
            StringContent data = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            return await Client.PostAsync($"api/MeterData/GetTimeSeries/{dateFrom.ToString("yyyy-MM-dd")}/{dateTo.ToString("yyyy-MM-dd")}/{aggregation}", data);
        }

        /// <summary>
        /// This request is used for querying meter readings for one or more (linked/related) metering points for a specified period.
        /// </summary>
        public async Task<HttpResponseMessage> GetMeterReadings(string[] meteringPointIds, DateTime dateFrom, DateTime dateTo)
        {
            if (Client == null) 
                return ClientResponse;

            if (meteringPointIds == null || !meteringPointIds.Any())
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Meteringpoint input is missing" };

            var json = new JObject(new JProperty("meteringPoints", new JObject(new JProperty("meteringPoint", new JArray(meteringPointIds)))));
            StringContent data = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            return await Client.PostAsync($"api/MeterData/GetMeterReadings/{dateFrom.ToString("yyyy-MM-dd")}/{dateTo.ToString("yyyy-MM-dd")}", data);
        }
    }

    public enum TimeAggregation
    {
        Actual,
        Quarter,
        Hour,
        Day,
        Month,
        Year
    }
}

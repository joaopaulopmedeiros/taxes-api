using Newtonsoft.Json;

using StackExchange.Redis;

using Taxes.Api.Extensions;
using Taxes.Api.Models;
using Taxes.Api.Requests;
using Taxes.Api.Responses;

namespace Taxes.Api.Services
{
    public class SelicTaxSearchService : ITaxesSearchService
    {
        private readonly IDatabase _db;
        private readonly HttpClient _httpClient;

        public SelicTaxSearchService(IDatabase db, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _httpClient = httpClientFactory.CreateClient("Selic");
        }

        /// <summary>
        /// Get selic taxes from bcb, map response
        /// and retrieve results
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TaxSearchResponse?> SearchByAsync(TaxSearchRequest request)
        {
            var key = request.ToSelicCacheKey();

            var cachedValue = await _db.StringGetAsync(key);

            var content = cachedValue.ToString();

            if (string.IsNullOrEmpty(content))
            {
                var queryParams = request.ToSelicQueryParamsString();

                var response = await _httpClient.GetAsync(queryParams);

                response.EnsureSuccessStatusCode();

                content = await response.Content.ReadAsStringAsync();

                await _db.StringSetAsync(key, content);
            }

            var taxes = JsonConvert.DeserializeObject<List<Selic>>(content);

            if (taxes == null) return null;

            return taxes.ToApiResponse();
        }
    }
}

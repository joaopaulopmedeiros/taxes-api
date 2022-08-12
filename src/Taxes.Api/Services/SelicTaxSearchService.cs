using Newtonsoft.Json;

using Taxes.Api.Extensions;
using Taxes.Api.Models;
using Taxes.Api.Requests;
using Taxes.Api.Responses;

namespace Taxes.Api.Services
{
    public class SelicTaxSearchService
    {
        private readonly HttpClient _httpClient;

        public SelicTaxSearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get selic taxes from bcb, map response
        /// and retrieve results
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TaxSearchResponse?> SearchByAsync(TaxSearchRequest request)
        {
            var queryParams = request.ToSelicQueryParamsString();
 
            var response = await _httpClient.GetAsync(queryParams);

            string content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) throw new Exception($"Unable to search selic tax on bcb. Response content: {content}");

            var taxes = JsonConvert.DeserializeObject<List<Selic>>(content);

            if (taxes == null) return null;

            return taxes.ToApiResponse();
        }
    }
}

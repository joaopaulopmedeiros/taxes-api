using Taxes.Api.Requests;
using Taxes.Api.Responses;

namespace Taxes.Api.Services
{
    public interface ITaxesSearchService
    {
        Task<TaxSearchResponse?> SearchByAsync(TaxSearchRequest request);
    }
}

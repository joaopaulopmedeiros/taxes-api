using Taxes.Api.Requests;
using Taxes.Api.Responses;

namespace Taxes.Api.Services
{
    public interface ITaxSearchService
    {
        Task<TaxSearchResponse?> SearchByAsync(TaxSearchRequest request);
    }
}

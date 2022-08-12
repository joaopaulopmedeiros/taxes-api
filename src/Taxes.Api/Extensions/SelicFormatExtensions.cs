using Microsoft.AspNetCore.WebUtilities;

using Taxes.Api.Models;
using Taxes.Api.Requests;
using Taxes.Api.Responses;

namespace Taxes.Api.Extensions
{
    public static class SelicFormatExtensions
    {
        public static string ToSelicQueryParamsString(this TaxSearchRequest request)
            => $"?formato=json&dataInicial={request.StartAt.ToString("dd/MM/yyyy")}&dataFinal={request.StartAt.ToString("dd/MM/yyyy")}";

        public static string ToSelicCacheKey(this TaxSearchRequest request)
            => $"selic:{request.StartAt.ToString("dd/MM/yyyy")}:{request.EndAt.ToString("dd/MM/yyyy")}";

        public static TaxSearchResponse ToApiResponse(this List<Selic> models)
        {
            var resp = new TaxSearchResponse();

            models.ForEach(m => resp.Add(new Tax(m.Data, m.Valor, TaxType.Selic)));

            return resp;
        }
    }
}

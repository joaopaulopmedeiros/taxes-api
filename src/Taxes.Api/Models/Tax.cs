using System.Linq;

namespace Taxes.Api.Models
{
    public class Tax
    {
        public Tax(string date, string? valor, TaxType selic)
        {
            Date = DateTime.Parse(string.Join("-", date.Replace("/","-").Split().Reverse()));
            Value = Convert.ToDouble(valor);
            Type = TaxType.Selic;
        }

        public TaxType Type { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }

    public enum TaxType
    {
        Selic = 1,
        Ipca = 2
    }
}

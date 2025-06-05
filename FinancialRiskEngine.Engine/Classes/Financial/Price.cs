using FinancialRiskEngine.Engine.Classes.Interfaces;
using System.Text.Json.Serialization;
using static FinancialRiskEngine.Engine.Enums.Enums;

namespace FinancialRiskEngine.Engine.Classes.Financial
{
    public class Price : IPrice
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("open")]
        public double Open { get; set; }

        [JsonPropertyName("close")]
        public double Close { get; set; }

        [JsonPropertyName("return")]
        public double Return => (Open == 0) ? 0 : (Close - Open) / Open;

        [JsonPropertyName("logReturn")]
        public double LogReturn => (Open <= 0 || Close <= 0) ? 0 : Math.Log(Close / Open);

        public VolatilityScenario VolatilityScenario { get; set; } = VolatilityScenario.NORMAL;

        public Price() { }
        public Price(DateTime date, double open, double close, VolatilityScenario volatilityScenario = VolatilityScenario.NORMAL)
        {
            Date = date;
            Open = open;
            Close = close;
            VolatilityScenario = volatilityScenario;
        }
    }

    public class ExtendedPrice : Price
    {
        public string Ticker { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Volume { get; set; }
    }
}

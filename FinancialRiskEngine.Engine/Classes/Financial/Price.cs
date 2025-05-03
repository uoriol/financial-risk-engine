using FinancialRiskEngine.Engine.Classes.Interfaces;
using static FinancialRiskEngine.Engine.Enums.Enums;

namespace FinancialRiskEngine.Engine.Classes.Financial
{
    public class Price : IPrice
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double Return => (Open == 0) ? 0 : (Close - Open) / Open;

        public double Close { get; set; }
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

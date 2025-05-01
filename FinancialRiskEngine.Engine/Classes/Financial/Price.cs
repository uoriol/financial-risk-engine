using FinancialRiskEngine.Engine.Classes.Interfaces;
using static FinancialRiskEngine.Engine.Enums.Enums;

namespace FinancialRiskEngine.Engine.Classes.Financial
{
    public class Price : IPrice
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double Return { get; set; }

        public double Close => Open + Open * Return;
        public VolatilityScenario VolatilityScenario { get; set; } = VolatilityScenario.NORMAL;

        public Price() { }
        public Price(DateTime date, double open, double returnValue, VolatilityScenario volatilityScenario = VolatilityScenario.NORMAL)
        {
            Date = date;
            Open = open;
            Return = returnValue;
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

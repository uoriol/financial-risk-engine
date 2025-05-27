using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Classes.Financial
{
    public class PortfolioSummary
    {
        public string Name { get; set; }

        [JsonPropertyName("expectedReturn")]
        public double ExpectedYearlyReturn { get; set; }

        [JsonPropertyName("standardDeviation")]
        public double ExpectedYearlyStandatdDeviation { get; set; }

        public PortfolioSummary() { }

        public PortfolioSummary(string portfolioName, double expectedYearlyReturn, double expectedYearlyStandatdDeviation)
        {
            Name = portfolioName;
            ExpectedYearlyReturn = expectedYearlyReturn;
            ExpectedYearlyStandatdDeviation = expectedYearlyStandatdDeviation;
        }

        public PortfolioSummary Clone()
        {
            return new PortfolioSummary()
            {
                ExpectedYearlyReturn = ExpectedYearlyReturn,
                ExpectedYearlyStandatdDeviation = ExpectedYearlyStandatdDeviation
            };
        }
    }
    public class Portfolio
    {
    }
}

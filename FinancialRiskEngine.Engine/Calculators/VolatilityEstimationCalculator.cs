using FinancialRiskEngine.Engine.Classes.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Calculators
{
    public class EWMAEstimation
    {
        public DateTime Date { get; set; }
        public double Volatility { get; set; }
    }
    public static class VolatilityEstimationCalculator
    {
        public static List<EWMAEstimation> ComputeEWMA(List<Price> prices, double lambda = 0.9, int nDays = 3)
        {
            var volatilityEstimations = new List<EWMAEstimation>();
            prices = prices.OrderBy(p => p.Date).ToList();
            volatilityEstimations.Add(new EWMAEstimation
            {
                Date = prices[0].Date,
                Volatility = 0 // Initial volatility is zero
            });
            for (int t = 1; t < prices.Count; t++)
            {
                double vol = 0;
                for(int i = 1; i <= Math.Min(t, nDays); i++)
                {
                    vol += Math.Pow(lambda, i-1) * Math.Pow(prices[t-i].Return, 2);
                }
                var ewma = (1 - lambda) * vol;
                volatilityEstimations.Add(new EWMAEstimation
                {
                    Date = prices[t].Date,
                    Volatility = Math.Sqrt(ewma)
                });
            }
            return volatilityEstimations;
        }
    }
}

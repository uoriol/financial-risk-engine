using FinancialRiskEngine.Engine.Classes.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Calculators
{
    public static class PerformanceMetricsCalculator
    {
        public static double ComputeSharpeRatio(List<Price> prices, double riskFreeRate)
        {
            // (return - rfr) / volatility
            return GetYearlyExcessReturn(prices, riskFreeRate) / GetYearlyVolatility(prices);
        }

        public static double ComputeSortinoRatio(List<Price> prices, double riskFreeRate)
        {
            // (return - rfr) / volatility of downside
            double dailyDownsideVariance = 0;
            double dailyMeanReturn = prices.Average(x => x.Return);
            var downsideReturns = prices.Where(x => x.Return < GetDailyRiskFreeRate(riskFreeRate)).ToList();

            if(downsideReturns.Count == 0)
            {
                return double.PositiveInfinity;
            }

            foreach (var price in downsideReturns)
            {
                dailyDownsideVariance += Math.Pow((price.Return - GetDailyRiskFreeRate(riskFreeRate)), 2);
            }
            dailyDownsideVariance = dailyDownsideVariance / (downsideReturns.Count() - 1);
            double dailyDownsideVolatility = Math.Sqrt(dailyDownsideVariance);

            return GetYearlyExcessReturn(prices, riskFreeRate) / ExtrapolateYearlyVolatility(dailyDownsideVolatility);
        }


        // Move to helper (?)
        private static double GetDailyRiskFreeRate(double riskFreeRate, int tradingDays = 252)
        {
            return riskFreeRate / tradingDays;
        }

        public static double ComputeTreynorRatio(List<Price> prices, double beta, double riskFreeRate)
        {
            // (return - rfr) / beta
            return GetYearlyExcessReturn(prices, riskFreeRate) / beta;
        }

        public static double ComputeJensenAlpha(List<Price> prices, double beta, double riskFreeRate)
        {
            // return - [rfr + Beta*(excess return)]

            // excess return <- return - rfr

            throw new NotImplementedException();
        }

        private static double GetYearlyExcessReturn(List<Price> prices, double riskFreeRate)
        {
            return ExtrapolateYearlyReturn(prices.Average(x => x.Return)) - riskFreeRate;
        }

        private static double GetYearlyVolatility(List<Price> prices)
        {
            double dailyVariance = 0;
            var meanReturns = prices.Average(x => x.Return);
            foreach (var price in prices)
            {
                dailyVariance += Math.Pow((price.Return - meanReturns), 2);
            }
            dailyVariance = dailyVariance / (prices.Count - 1);

            var dailyVolatility = Math.Sqrt(dailyVariance);

            return ExtrapolateYearlyVolatility(dailyVolatility);
        }

        public static double ComputeInformationRatio()
        {
            throw new NotImplementedException();
        }

        private static double ExtrapolateYearlyReturn(double dailyReturn, int tradingDays = 252)
        {
            // Assuming i.i.d
            return Math.Pow(1 + dailyReturn, 252) - 1;
        }

        private static double ExtrapolateYearlyVolatility(double dailyVolatility, int tradingDays = 252)
        {
            // Assuming i.i.d
            return dailyVolatility * Math.Sqrt(tradingDays);
        }

        private static double AreReturnsUncorrelated(List<Price> prices)
        {
            // Durbin-Watson test
            double dwNumerator = 0;
            for (int i = 1; i < prices.Count; i++)
            {
                dwNumerator += Math.Pow((prices[i].Return - prices[i - 1].Return), 2);
            }

            return dwNumerator / prices.Sum(x=> Math.Pow(x.Return, 2));
        }

        private static bool InterpretDWTest(double score)
        {
            return score > 1 && score < 3;
        }
    }
}

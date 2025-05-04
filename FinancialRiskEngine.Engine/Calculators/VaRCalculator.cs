using FinancialRiskEngine.Engine.Classes.Financial;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinancialRiskEngine.Engine.Enums.Enums;

namespace FinancialRiskEngine.Engine.Calculators
{
    public enum VaRCalculationType
    {
        Historic = 1,
        Parametric = 2,
        MonteCarlo = 3
    }
    public static class VaRCalculator
    {
        public static double ComputeValueAtRisk(List<Price> prices, double confidenceLevel = 0.99, int ndays = 1)
        {
            // Check for normality first
            return ComputeVaRNDays(ComputeValueAtRisk(prices, confidenceLevel), ndays);
        }

        public static double ComputeValueAtRisk(List<Price> prices, double confidenceLevel = 0.99)
        {
            // TODO : Take into account and handle edge cases
            var orderedReturns = prices.OrderBy(r => r.Return).ToList();

            // What position should we take?
            // Position = N * (1 - ConfidenceLevel)
            double position = prices.Count() * (1 - confidenceLevel);

            // If we get a decimal, we can:
            // - Interpolate (returns[i] + remainder * (return[i+1] - return[i])
            // - Round down or up
            int previousPosition = (int)Math.Floor(position);
            double remainder = position - previousPosition;

            double returnVaR = (double)orderedReturns[previousPosition].Return + remainder * ((double)orderedReturns[previousPosition + 1].Return - (double)orderedReturns[previousPosition].Return);

            return (prices.Last().Close * returnVaR) * -1;
        }

        public static double ComputeVaRNDays(double VaR, int ndays = 10)
        {
            // Only appropiate when assuming independent and identically distributed results
            return VaR * Math.Sqrt(ndays);
        }
    }
}

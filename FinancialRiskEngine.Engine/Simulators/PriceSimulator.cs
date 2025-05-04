using FinancialRiskEngine.Engine.Classes.Financial;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Providers.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Simulators
{
    public static class PriceSimulator
    {
        // I want to have several ways of simulating prices
        // The first one should be a simple Monte Carlo simulation with normal distribution
        // The second one should be a normal distribution with GARCH process for the volatility
        // The third one I would like to use Markov Chain to jump between states as we simulate

        // We can additionally consider implementing a random walk simulation, or other models

        public static async Task<List<Price>> GetSimulatedPricesNormalDistributionOptimizedAsync(int n = 1000, double mean = 0.001, double std = 0.008, double open = 100)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // Fill array with n samples in one call
            double[] returns = new double[n];
            Normal normal = new Normal(mean, std);
            normal.Samples(returns);

            var prices = new List<Price>{
                new Price(DateTime.Now, open, Math.Max(open + (open * returns[0]), 0))
            };

            for (int i = 1; i < n; i++)
            {
                prices.Add(new Price()
                {
                    Date = GetNextTradingDay(prices[i - 1].Date),
                    Open = prices[i - 1].Close,
                    Close = prices[i - 1].Close + (prices[i - 1].Close * returns[i])
                });
            }
            sw.Stop();
            Console.WriteLine($"Elapsed milliseconds (optimized): {sw.ElapsedMilliseconds}");
            return prices;
        }

        public static async Task<List<Price>> GetSimulatedPricesNormalDistributionAsync(int n = 1000, double mean = 0.001, double std = 0.008, double open = 100)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool hasReachedZero = false;
            var prices = new List<Price>{
                new Price(DateTime.Now, open, Math.Max(open + (open * GetRandomNormalValue()), 0))
            };
            for (int i = 1; i < n; i++)
            {
                prices.Add(new Price()
                {
                    Date = GetNextTradingDay(prices[i - 1].Date),
                    Open = prices[i - 1].Close,
                    Close = prices[i - 1].Close + (prices[i - 1].Close * GetRandomNormalValue(mean, std))
                });
            }
            stopwatch.Stop();
            Console.WriteLine($"Elapsed milliseconds: {stopwatch.ElapsedMilliseconds}");
            return prices;
        }

        public static double GetRandomNormalValue(double mean = 0.001, double std = 0.008, Normal normalDist = null)
        {
            // Only create the normal distribution once
            normalDist = normalDist ?? new Normal(mean, std);
            return normalDist.Sample();
        }

        public static DateTime GetNextTradingDay(DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            }
            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);

            return date;
        }
    }
}

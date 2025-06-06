using FinancialRiskEngine.Engine.Calculators;
using FinancialRiskEngine.Engine.Classes.Financial;
using FinancialRiskEngine.Engine.Classes.Interfaces;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Providers.LinearAlgebra;
using MathNet.Numerics.Random;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static FinancialRiskEngine.Engine.Enums.Enums;

namespace FinancialRiskEngine.Engine.Simulators
{
    public static class PriceSimulator
    {
        // I want to have several ways of simulating prices
        // The first one should be a simple Monte Carlo simulation with normal distribution
        // The second one should be a normal distribution with GARCH process for the volatility
        // The third one I would like to use Markov Chain to jump between states as we simulate

        // We can additionally consider implementing a random walk simulation, or other models

        public static async Task<List<Price>> GetSimulatedPricesMarkovChainProcess(int n = 500, VolatilityScenario initialState = VolatilityScenario.NORMAL, double open = 100)
        {
            var prices = new List<Price>();
            (double currentReturn, VolatilityScenario currentVolatilityScenario) = GetRandomMarkovOutput(VolatilityScenario.NORMAL);
            prices.Add(new Price()
            {
                Date = DateTime.Now,
                Open = open,
                Close = open + (open * currentReturn),
                VolatilityScenario = currentVolatilityScenario
            });
            for (int i = 1; i <= n; i++)
            {
                (currentReturn, currentVolatilityScenario) = GetRandomMarkovOutput(prices[i - 1].VolatilityScenario);
                prices.Add(new Price()
                {
                    Date = DateTime.Now.AddDays(i),
                    Open = prices[i - 1].Close,
                    Close = prices[i - 1].Close + (prices[i - 1].Close * currentReturn),
                    VolatilityScenario = currentVolatilityScenario
                });
            }
            return prices;
        }

        public static (double, VolatilityScenario) GetRandomMarkovOutput(VolatilityScenario precedentVolatilityScenario)
        {
            // First we get the new markov state
            VolatilityScenario newVolatilityScenario = GetMarkovScenario(precedentVolatilityScenario);
            // Depending on the current markov state, we use a different normal distribution to obtain the random value
            // The parameters of the normal distribution should also be parametrized so that the user can set whatever value they want
            double mean = newVolatilityScenario == VolatilityScenario.STRESS ? -0.001 : 0.001;
            double sd = newVolatilityScenario == VolatilityScenario.STRESS ? 0.03 : newVolatilityScenario == VolatilityScenario.HIGH_VOLATILITY ? 0.016 : 0.008;
            return (GetRandomNormalValue(mean, sd), newVolatilityScenario);
        }

        public static VolatilityScenario GetMarkovScenario(VolatilityScenario precedentVolatilityScenario)
        {
            // We should improve performance (for now we prioritize readability)
            // We hard-code it for now
            // This can be more easiliy displayed with a probability matrix
            // In the future we should let the user introduce as many states as necessary (of course validating that the sum are always 1 etc)
            Random random = new Random();
            decimal stochasticValue = random.NextDecimal();
            // NORMAL
            if (precedentVolatilityScenario == VolatilityScenario.NORMAL)
            {
                return stochasticValue switch
                {
                    <= 0.001m => VolatilityScenario.STRESS,
                    > 0.001m and <= (0.074m + 0.001m) => VolatilityScenario.HIGH_VOLATILITY,
                    _ => VolatilityScenario.NORMAL
                };
            }
            // HIGH VOLATILITY
            if (precedentVolatilityScenario == VolatilityScenario.HIGH_VOLATILITY)
            {
                return stochasticValue switch
                {
                    <= 0.05m => VolatilityScenario.STRESS,
                    > 0.05m and <= (0.3m + 0.05m) => VolatilityScenario.HIGH_VOLATILITY,
                    _ => VolatilityScenario.NORMAL
                };
            }
            // STRESS
            return stochasticValue switch
            {
                <= 0.25m => VolatilityScenario.NORMAL,
                > 0.25m and <= (0.30m + 0.25m) => VolatilityScenario.STRESS,
                _ => VolatilityScenario.HIGH_VOLATILITY
            };
        }

        public static async Task<List<List<Price>>> GetMultipleSimulatedPricesNormalDistributionOptimizedAsync(int n = 1000, double mean = 0.001, double std = 0.008, double open = 100, int runs = 1)
        {
            var result = new List<List<Price>>();
            var tasks = new List<Task>();
            for (int i = 0; i < runs; i++)
            {
                tasks.Add(Task.Run(async () => {
                    var simulation = await GetSimulatedPricesNormalDistributionOptimizedAsync(n, mean, std, open);
                    result.Add(simulation);
                }));
            }
            await Task.WhenAll(tasks);
            return result;
        }

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

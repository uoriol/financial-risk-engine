using FinancialRiskEngine.Engine.Calculators;
using FinancialRiskEngine.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinancialRiskEngine.Engine.Enums.Enums;

namespace FinancialRiskEngine.Engine.Classes.Mathematical
{
    public class BinomialTree
    {
        public double InitialPrice { get; set; } // S_0, the initial stock price
        public BinomialStep Root { get; set; }
        public double UpFactor { get; set; } // Factor by which the price increases (u)
        public double DownFactor { get; set; } // Factor by which the price decreases (d)
    }
    public class BinomialStep
    {
        public double OptionPrice { get; set; }
        public double StockPrice { get; set; }
        public BinomialStep? StepUp { get; set; }
        public BinomialStep? StepDown { get; set; }
        public BinomialStep(double price, double upFactor, double downFactor, int steps, bool computePriceOnCreate = false, double? probability = null)
        {
            StockPrice = price;

            if (steps >= 1)
            {
                StepUp = new BinomialStep(price * upFactor, upFactor, downFactor, steps - 1, computePriceOnCreate, probability);
                StepDown = new BinomialStep(price * downFactor, upFactor, downFactor, steps - 1, computePriceOnCreate, probability);
            }
            else
            {
                StepUp = null;
                StepDown = null;
            }

            if (computePriceOnCreate)
            {
                // Compute
            }
        }

        public void SetOptionPriceNoArbitrage(double strikePrice, double riskFreeRate, double stepTime, double continuousDividend = 0, OptionStyle style = OptionStyle.EUROPEAN)
        {
            if (StepUp == null || StepDown == null)
            {
                OptionPrice = Math.Max(0, StockPrice - strikePrice); // If we are at the last step, we can calculate the option price directly
                return;
            }

            // We ask the next step to first calculate their option price
            // and then calculate our own option price based on the next step's prices.
            StepUp.SetOptionPriceNoArbitrage(strikePrice, riskFreeRate, stepTime, continuousDividend, style);
            StepDown.SetOptionPriceNoArbitrage(strikePrice, riskFreeRate, stepTime, continuousDividend, style);

            // Now we calculate the price knowing we have the option prices of the next step.

            // Step 1, we build a risk-free portfolio
            double delta = 0;

            // We know that delta*priceUp - priceOptionUp should be equal to delta*priceDown - priceOptionDown
            // We can rearrange this to find delta:

            if (StepUp != null && StepDown != null)
            {
                delta = (StepUp.OptionPrice - StepDown.OptionPrice) / (StepUp.StockPrice - StepDown.StockPrice);
            }

            // Knowing this composition, we can calculate the payoff of either the up movement or the down movement.
            // Because it should be the same!!

            double payOff = delta * StepUp.StockPrice - StepUp.OptionPrice;

            // Knowing the payoff after the step, we need to discount it to present time

            double payOffDiscounted = InterestRateCalculator.GetDiscountValue((riskFreeRate - continuousDividend), payOff, stepTime);

            // We know the portfolio value today, the stock price today, so we calculate the option price today.
            // payOffDiscounted = delta*stockPrice - optionPrice
            // If we rearranging this, we get:
            OptionPrice = Math.Max(0, delta * StockPrice - payOffDiscounted);

            // If the style is American, we need to check if early exercise is better
            if (style == OptionStyle.AMERICAN)
            {
                double earlyExerciseValue = Math.Max(0, StockPrice - strikePrice);
                OptionPrice = Math.Max(OptionPrice, earlyExerciseValue);
            }
        }
    }
}

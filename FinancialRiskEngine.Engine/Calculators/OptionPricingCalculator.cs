using FinancialRiskEngine.Engine.Classes.Mathematical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Calculators
{
    public static class OptionPricingCalculator
    {
        public static double ComputeOptionPrice(BinomialTree tree, double strikePrice, double riskFreeRate = 0.04, double stepTime = 0.25)
        {
            // My idea here is that in order to apply the non-arbitrage argument to value the option, 
            // we need to start at the end of the tree and work our way back to the root.
            // This way, we calculate the value of the option at each node, which is a necessary information
            // to compute the value of the option at the previous node, and all the way until T=0.
            // If we are at the last step, it is easy to define the value of the option after the step,
            // and thus can apply the no-arbitrage principle to discount the option price after having 
            // built a risk/free portfolio.
            tree.Root.SetOptionPriceNoArbitrage(strikePrice, riskFreeRate, stepTime);
            return tree.Root.OptionPrice;
        }
    }
}

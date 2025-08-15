using FinancialRiskEngine.Engine.Classes.Mathematical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Generators
{
    public static class BinomialTreeGenerator
    {

        public static BinomialTree GenerateBinomialTree(
            double initialPrice,
            double upFactor,
            double downFactor,
            int steps)
        {
            if (steps <= 0)
                throw new ArgumentException("Number of steps must be greater than zero.");
            var tree = new BinomialTree
            {
                InitialPrice = initialPrice,
                UpFactor = upFactor,
                DownFactor = downFactor,

                Root = new BinomialStep(initialPrice, upFactor, downFactor, steps)
            };
            return tree;
        }
    }
}

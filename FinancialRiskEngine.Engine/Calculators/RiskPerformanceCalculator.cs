using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Calculators
{
    public static class RiskPerformanceCalculator
    {
        public static double ComputeSharpeRatio(double returnValue, double riskFreeRate, double std)
        {
            if (std == 0)
            {
                return 0;
            }

            return (returnValue - riskFreeRate) / std;
        }
    }
}

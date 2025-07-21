using FinancialRiskEngine.Engine.Classes.Financial;
using static FinancialRiskEngine.Engine.Helpers.DateHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static FinancialRiskEngine.Engine.Enums.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinancialRiskEngine.Engine.Calculators
{
    public static class IRRCalculator
    {
        public static (double, List<double>, List<double>) ComputeIRR(List<CashFlow> CFs, double tolerance = 0.0001, double lowerAssumption = -0.999, double higherAssumption = 10, int maxIterations = 10000)
        {
            double assumption1 = lowerAssumption;
            double assumption2 = higherAssumption;
            int currentIteration = 0;
            double slideValue = 1.0;

            List<double> assumptions1 = new List<double>();
            List<double> assumptions2 = new List<double>();

            var analysisDate = CFs.Select(c => c.Date).Min();

            bool fixedInterval = false;
            // Ensure the real return is between assumption 1 and assumption2
            while (currentIteration < maxIterations)
            {
                assumptions1.Add(assumption1);
                assumptions2.Add(assumption2);

                var NPV1 = ComputeNPV(CFs, assumption1, analysisDate);
                var NPV2 = ComputeNPV(CFs, assumption2, analysisDate);

                if (NPV1 >= 0 && 0 >= NPV2)
                {
                    fixedInterval = true;
                    break;
                }

                // Both too high or too low
                if (NPV1 > 0 && NPV2 > 0)
                {
                    // Both too high -> the IRR should increase (to reduce NPV to zero) -> increase both
                    assumption1 += slideValue;
                    assumption2 += slideValue;
                    currentIteration++;
                    continue;
                }

                if (NPV1 < 0)
                {
                    // If we know that NPV1 is lower than -0.999, we should return early to avoid a square root of a negative number
                    return (-1, assumptions1, assumptions2);
                }

                if (NPV2 > 0)
                {
                    assumption2 += slideValue;
                    currentIteration++;
                    continue;
                }

                currentIteration++;
            }

            if (!fixedInterval)
            {
                throw new Exception("Unable to define an interval");
            }

            currentIteration = 0;

            do
            {
                currentIteration++;

                if (Math.Abs(assumption2 - assumption1) <= tolerance)
                {
                    return ((assumption1 + assumption2) / 2, assumptions1, assumptions2);
                }

                assumptions1.Add(assumption1);
                assumptions2.Add(assumption2);

                double midPoint = (double)((assumption1 + assumption2) / 2);
                var estimatedPrice = ComputeNPV(CFs, midPoint, analysisDate);

                if (estimatedPrice > 0)
                {
                    // We need to discount more !
                    // Increase assumption 1
                    assumption1 = midPoint;
                    continue;
                }

                if (estimatedPrice < 0)
                {
                    // We are discounting too much, so the IRR should be lower
                    assumption2 = midPoint;
                    continue;
                }
            } while (currentIteration < maxIterations);

            throw new Exception("Unable to converge value");
        }

        private static double ComputeIRRWithLibrary()
        {
            throw new NotImplementedException("We can implement the same functionality using libraries and comparing our calculator output with the library output, plus runtime differences.");
        }

        private static double ComputeNPV(List<CashFlow> CFs, double irr_estimation, DateTime initialDate)
        {
            double npv = 0;
            foreach (CashFlow cf in CFs)
            {
                npv += (cf.Amount / Math.Pow((1 + irr_estimation), GetYearFraction(cf.Date, initialDate)));
            }
            return npv;
        }
    }
}

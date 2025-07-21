using FinancialRiskEngine.Engine.Classes.Financial;
using static FinancialRiskEngine.Engine.Helpers.DateHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Calculators
{
    public static class DurationCalculator
    {
        public static double ComputeTimeToMaturity(List<CashFlow> CFs, DateTime? analysisDate = null)
        {
            var _analysisDate = analysisDate ?? DateTime.Today;
            return GetYearFraction(CFs.Max(x => x.Date).Date, _analysisDate);
        }
        public static (double, double) ComputeDuration(List<CashFlow> CFs, double? bondDirtyPrice = null, DateTime? analysisDate = null)
        {
            // Simply, duration is the weighted average maturity for all future cashflows (discounted)

            var _analysisDate = analysisDate == null ? DateTime.Today : (DateTime)analysisDate;
            var _futureCFs = CFs.Where(x => x.Date > _analysisDate && x.Amount > 0).ToList(); // We also remove the first initial payment

            if (_futureCFs.Count == 0) {
                throw new Exception("No future cashflows available.");
            }

            // If no dirty price provided, we assume it is selling at-par.
            var _bondDirtyPrice = bondDirtyPrice == null ? Math.Abs(CFs.First().Amount) : (double)bondDirtyPrice;

            var _futureCashFlowsWithPayment = new List<CashFlow>()
            {
                new CashFlow(_analysisDate, _bondDirtyPrice * -1)
            };

            _futureCashFlowsWithPayment.AddRange(_futureCFs);

            (var YTM, _, _) = IRRCalculator.ComputeIRR(_futureCashFlowsWithPayment, tolerance: 0.0000001);
            var _totalAmount = _futureCFs.Sum(x => x.GetDiscountedAmount(YTM, _analysisDate));

            var duration = 0.0;
            
            foreach (var futureCF in _futureCFs)
            {
                // Get weight of CF
                var _weight = futureCF.GetDiscountedAmount(YTM, _analysisDate) / _totalAmount;
                duration += _weight * GetYearFraction(futureCF.Date, _analysisDate);
            }

            return (duration, YTM);
        }

        public static double ComputeModifiedDuration(double duration, double YTM) 
        {
            return duration / (1 + YTM);
        }
    }
}

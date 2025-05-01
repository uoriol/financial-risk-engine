using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinancialRiskEngine.Engine.Enums.Enums;

namespace FinancialRiskEngine.Engine.Calculators
{
    public static class InterestRateCalculator
    {
        public static decimal GetFutureValue(decimal principal, decimal rate, decimal years, CompoudingFrequency frequency)
        {
            if(frequency == CompoudingFrequency.CONTINUOUS)
            {
                return principal * (decimal)Math.Pow((double)Math.E, (double)(rate * years));
            }
            int frequencyValue = 0;
            switch (frequency)
            {
                case CompoudingFrequency.DAILY:
                    frequencyValue = 365;
                    break;
                case CompoudingFrequency.WEEKLY:
                    frequencyValue = 52;
                    break;
                case CompoudingFrequency.MONTHLY:
                    frequencyValue = 12;
                    break;
                case CompoudingFrequency.QUARTERLY:
                    frequencyValue = 4;
                    break;
                case CompoudingFrequency.SEMI_ANNUALLY:
                    frequencyValue = 2;
                    break;
                case CompoudingFrequency.ANNUALLY:
                    frequencyValue = 1;
                    break;
            }
            decimal amount = principal * (decimal)Math.Pow((double)(1 + rate / frequencyValue), (double)(frequencyValue * years));
            return amount;
        }
    }
}

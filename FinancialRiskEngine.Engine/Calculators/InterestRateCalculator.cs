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
        public static decimal GetFutureValue(decimal principal, decimal rate, decimal years, CompoundingFrequency frequency)
        {
            if(frequency == CompoundingFrequency.CONTINUOUS)
            {
                return principal * (decimal)Math.Pow((double)Math.E, (double)(rate * years));
            }
            int frequencyValue = GetFrequencyValue(frequency);
            decimal amount = principal * (decimal)Math.Pow((double)(1 + rate / frequencyValue), (double)(frequencyValue * years));
            return amount;
        }

        public static decimal GetEquivalentRate(decimal rate1, CompoundingFrequency frequency1, CompoundingFrequency frequency2)
        {
            if(frequency1 == frequency2)
            {
                return rate1;
            }

            if(frequency1 == CompoundingFrequency.CONTINUOUS)
            {
                int frequencyValue = GetFrequencyValue(frequency2);
                return (decimal)(frequencyValue * (Math.Exp((double)(rate1 / frequencyValue)) - 1));
            }

            if(frequency2 == CompoundingFrequency.CONTINUOUS)
            {
                int frequencyValue = GetFrequencyValue(frequency1);
                return frequencyValue * (decimal)Math.Log((double)(1 + (rate1 / frequencyValue)));
            }

            int frequencyValue1 = GetFrequencyValue(frequency1);
            int frequencyValue2 = GetFrequencyValue(frequency2);

            return (decimal)(frequencyValue2 * ((Math.Pow((double)(1 + rate1 / frequencyValue1), (double)frequencyValue1 / frequencyValue2) - 1)));
        }

        public static int GetFrequencyValue(CompoundingFrequency frequency)
        {
            switch (frequency)
            {
                case CompoundingFrequency.DAILY:
                    return 365;
                    break;
                case CompoundingFrequency.WEEKLY:
                    return 52;
                    break;
                case CompoundingFrequency.MONTHLY:
                    return 12;
                    break;
                case CompoundingFrequency.QUARTERLY:
                    return 4;
                    break;
                case CompoundingFrequency.SEMI_ANNUALLY:
                    return 2;
                    break;
                case CompoundingFrequency.ANNUALLY:
                    return 1;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

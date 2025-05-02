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

        public static int GetFrequencyValue(PaymentFrequency frequency)
        {
            switch (frequency)
            {
                case PaymentFrequency.DAILY:
                    return 365;
                    break;
                case PaymentFrequency.WEEKLY:
                    return 52;
                    break;
                case PaymentFrequency.MONTHLY:
                    return 12;
                    break;
                case PaymentFrequency.QUARTERLY:
                    return 4;
                    break;
                case PaymentFrequency.SEMI_ANNUALLY:
                    return 2;
                    break;
                case PaymentFrequency.ANNUALLY:
                    return 1;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static decimal GetBondYield(decimal par, decimal price, decimal payment, decimal paymentPeriods, PaymentFrequency paymentFrequency)
        {
            // Two assumptions
            // 1. The interest is compounded continously
            // 2. The current payment has just been made (so the next payment will be made next year)

            // Hull proposes the Newton-Raphson method
            // I will implement a custom method instead

            int maxIterations = 1000;
            int currentIteration = 0;
            decimal assumption1 = 0.0m;
            decimal assumption2 = 1.0m;
            decimal tolerance = 0.00001m;
            decimal slideValue = 1.0m;


            bool fixedInterval = false;
            // Ensure the real return is between assumption 1 and assumption2
            while ((currentIteration < maxIterations))
            {
                var price1 = GetPrice(payment, paymentPeriods, par, assumption1, paymentFrequency);
                var price2 = GetPrice(payment, paymentPeriods, par, assumption2, paymentFrequency);

                if (price1 >= price && price >= price2)
                {
                    fixedInterval = true;
                    break;
                }

                // The higher the price, the lower the yield
                if (price1 < price)
                {
                    // Reduce assumption 1
                    assumption1 -= slideValue;
                    currentIteration++;
                    continue;
                }

                if (price2 > price)
                {
                    assumption2 += slideValue;
                    currentIteration++;
                    continue;
                }
            }

            if (!fixedInterval)
            {
                throw new Exception("Unable to define an interval");
            }

            currentIteration = 0;

            do
            {
                if(Math.Abs(assumption2 - assumption1) <= tolerance)
                {
                    return (assumption1 + assumption2) / 2;
                }

                var midPoint = (assumption1 + assumption2) / 2;
                var estimatedPrice = GetPrice(payment, paymentPeriods, par, midPoint, paymentFrequency);

                if (estimatedPrice < price)
                {
                    // Increase assumption 1
                    assumption1 = midPoint;
                    currentIteration++;
                    continue;
                }

                if (estimatedPrice > price)
                {
                    assumption2 = midPoint;
                    currentIteration++;
                    continue;
                }
            } while (currentIteration < maxIterations);

            throw new Exception("Unable to converge value");
        }

        private static decimal GetPrice(decimal payment, decimal paymentPeriods, decimal par, decimal yield, PaymentFrequency paymentFrequency)
        {
            var totalPrice = 0m;

            // Discount each payment
            for(int i = 1; i <= paymentPeriods; i++)
            {
                totalPrice += payment * (decimal)Math.Exp((double)-yield * ((double)i/GetFrequencyValue(paymentFrequency)));
            }

            // Discount principal
            totalPrice += par * (decimal)Math.Exp((double)-yield * ((double)paymentPeriods/GetFrequencyValue(paymentFrequency)));

            return totalPrice;
        }
    }
}

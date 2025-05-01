using FinancialRiskEngine.Engine.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinancialRiskEngine.Engine.Enums.Enums;

namespace FinancialRiskEngine.Engine.Classes.Financial
{
    public class InterestRateParameters
    {
        public InterestRateParameters() { }

        public decimal InterestRate { get; set; } = 0.0m;
        public decimal Amount { get; set; }
        public decimal Years { get; set; }
        public CompoudingFrequency CompoudingFrequency { get; set; }

        public InterestRateParameters SetSampleValues()
        {
            InterestRate = 0.1m;
            Amount = 100.0m;
            Years = 1m;
            CompoudingFrequency = CompoudingFrequency.ANNUALLY;

            return this;
        }

        public decimal GetFutureValue()
        {
            return InterestRateCalculator.GetFutureValue(Amount, InterestRate, Years, CompoudingFrequency);
        }

        public bool HasValidValues()
        {
            if (Amount <= 0.0m) { return false; }
            if (Years <= 0.0m) { return false; }
            return true;
        }
    }
}

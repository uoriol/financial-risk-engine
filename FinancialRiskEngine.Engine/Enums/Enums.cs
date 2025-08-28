using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Enums
{
    public static class Enums
    {
        public enum MeasureDescription
        {
            Poor = 1,
            Acceptable = 2,
            VeryGood = 3,
            Excellent = 4
        }
        public enum ResultType
        {
            RETURN_PCT = 1,
            RETURN_AMOUNT = 2
        }
        public enum StochasticProcess
        {
            MONTE_CARLO = 1,
            MARKOV_CHAIN = 2
        }
        public enum VolatilityScenario
        {
            NORMAL = 1,
            HIGH_VOLATILITY = 2,
            STRESS = 3
        }

        public enum CompoundingFrequency
        {
            DAILY = 1,
            WEEKLY = 2,
            MONTHLY = 3,
            QUARTERLY = 4,
            SEMI_ANNUALLY = 5,
            ANNUALLY = 6,
            CONTINUOUS = 7
        }

        public enum PaymentFrequency
        {
            DAILY = 1,
            WEEKLY = 2,
            MONTHLY = 3,
            QUARTERLY = 4,
            SEMI_ANNUALLY = 5,
            ANNUALLY = 6
        }

        public static string GetVolatilityName(VolatilityScenario scenario)
        {
            if (scenario == VolatilityScenario.STRESS) { return "STRESS"; }
            if (scenario == VolatilityScenario.HIGH_VOLATILITY) { return "High Volatility"; }
            if (scenario == VolatilityScenario.NORMAL) { return "Normal"; }

            return "NA";
        }

        public enum OptionType
        {
            CALL = 1,
            PUT = 2
        }

        public enum OptionStyle
        {
            EUROPEAN = 1,
            AMERICAN = 2
        }
    }
}

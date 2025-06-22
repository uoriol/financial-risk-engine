using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Helpers
{
    public static class DateHelper
    {
        public static double GetYearFraction(DateTime date, DateTime reference)
        {
            return (date - reference).Days / 365.0;
        }
    }
}

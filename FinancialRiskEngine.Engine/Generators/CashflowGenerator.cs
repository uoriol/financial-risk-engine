using FinancialRiskEngine.Engine.Classes.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinancialRiskEngine.Engine.Enums.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinancialRiskEngine.Engine.Generators
{
    public static class CashflowGenerator
    {
        public static List<CashFlow> GenerateBondCashFlows(DateTime initialDate, double principal, double paymentAmount, PaymentFrequency paymentFrequency, DateTime maturityDate)
        {
            var CFs = new List<CashFlow>();
            var _currentDate = initialDate;
            var _principalReturned = false; 
            var _initialDateDay = initialDate.Day;
            // Initial CF
            CFs.Add(new CashFlow(initialDate, principal*-1, "Initial payment"));
            _currentDate = UpdateCurrentDate(_currentDate, paymentFrequency, _initialDateDay);
            while (_currentDate <= maturityDate)
            {
                CFs.Add(new CashFlow(_currentDate, paymentAmount, "Coupon payment"));
                if (_currentDate == maturityDate)
                {
                    CFs.Add(new CashFlow(_currentDate, principal, "Returned principal"));
                    _principalReturned = true;
                }
                _currentDate = UpdateCurrentDate(_currentDate, paymentFrequency, _initialDateDay);
            }

            if (!_principalReturned)
            {
                CFs.Add(new CashFlow(maturityDate, principal, "Returned principal"));
            }

            return CFs;
        }

        private static DateTime UpdateCurrentDate(DateTime date, PaymentFrequency frequency, int day)
        {
            switch (frequency) {
                case PaymentFrequency.DAILY:
                    return date.AddDays(1);
                case PaymentFrequency.WEEKLY: 
                    return date.AddDays(7);
                case PaymentFrequency.MONTHLY:
                    return UpdateMonth(date, day: day);
                case PaymentFrequency.QUARTERLY:
                    return UpdateMonth(date, 3, day);
                case PaymentFrequency.SEMI_ANNUALLY:
                    return UpdateMonth(date, 6, day: day);
                case PaymentFrequency.ANNUALLY:
                    return UpdateYear(date, day: day);
            }
            throw new ArgumentException();
        }

        private static DateTime UpdateMonth(DateTime date, int months = 1, int day = 1, bool checkDay = true)
        {
            if (!checkDay)
            {
                return date.AddMonths(months);
            }

            var nextMonth = date.AddMonths(months);
            return new DateTime(nextMonth.Year, nextMonth.Month, Math.Min(day, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month)));
        }

        private static DateTime UpdateYear(DateTime date, int years = 1, int day = 1, bool checkDay = true)
        {
            if (!checkDay)
            {
                return date.AddYears(years);
            }

            var nextMonth = date.AddYears(years);
            return new DateTime(nextMonth.Year, nextMonth.Month, Math.Min(day, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month)));
        }
    }
}
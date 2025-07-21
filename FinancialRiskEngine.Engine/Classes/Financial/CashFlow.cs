using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinancialRiskEngine.Engine.Helpers.DateHelper;

namespace FinancialRiskEngine.Engine.Classes.Financial
{
    public class CashFlow
    {
        public DateTime Date { get; set; } = DateTime.Today;
        public double Amount { get; set; }
        public string? Description { get; set; }

        public CashFlow()
        {

        }
        public CashFlow(DateTime date, double amount, string? description = null)
        {
            Date = date;
            Amount = amount;
            Description = description;
        }

        public double GetDiscountedAmount(double interestRate, DateTime date)
        {
            return this.Amount / Math.Pow(1 + interestRate, GetYearFraction(this.Date, date));
        }
    }
}

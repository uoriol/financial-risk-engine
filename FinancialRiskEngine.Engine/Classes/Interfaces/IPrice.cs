using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.Classes.Interfaces
{
    public interface IPrice
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double Return { get; set; }
    }
}

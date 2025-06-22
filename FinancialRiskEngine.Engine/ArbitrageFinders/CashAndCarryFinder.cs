using FinancialRiskEngine.Engine.Classes.Financial;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Engine.ArbitrageFinders
{
    public static class CashAndCarryFinder
    {
        public static List<string> FindCashAndCarryArbitrage(double spotPrice, List<FutureContract> contracts, double flatInterestRate, bool continuouslyCompounded = true)
        {
            var arbitrageOpportunities = new List<string>();
            foreach (var contract in contracts)
            {
                var pnl = continuouslyCompounded ? 
                    ComputePnLContinousCompounding(spotPrice, contract.TimeToMaturity, contract.Price, flatInterestRate)
                    :
                    ComputePnL(spotPrice, contract.TimeToMaturity, contract.Price, flatInterestRate);
                if (pnl > 0)
                {
                    arbitrageOpportunities.Add(DescribeArbitrage(spotPrice, pnl, contract, flatInterestRate));
                }
            }
            return arbitrageOpportunities;
        }

        private static double ComputePnLContinousCompounding(double spotPrice, double timeToMaturity, double futuresPrice, double interestRate)
        {
            // We are going to also calculate continuously compounding (used in FRM assumptionts) --- and interest rate is flat
            var costOfFinancing = spotPrice * Math.Exp(interestRate * timeToMaturity);
            return futuresPrice - costOfFinancing;
        }

        private static double ComputePnL(double spotPrice, double timeToMaturity, double futuresPrice, double interestRate)
        {
            // We are going to assume payments are made yearly and interest rate is flat
            var years = (int)Math.Floor(timeToMaturity);
            var remainingTime = timeToMaturity - years;
            var costOfFinancing = spotPrice;
            for(int i = 0; i < years; i++)
            {
                costOfFinancing += costOfFinancing * interestRate;
            }
            if (remainingTime > 0)
            {
                costOfFinancing += costOfFinancing * interestRate * remainingTime;
            }

            return futuresPrice - costOfFinancing;
        }

        private static string DescribeArbitrage(double spotPrice, double PnL, FutureContract contract, double interestRate)
        {
            return "Arbitrage opportunity found by selling the future contract at " +
                   $"{contract.Price} and buying the underlying asset at {spotPrice}. " +
                   $"The profit from this arbitrage is {PnL} after accounting for interest rate of {interestRate * 100}% " +
                   $"over the time to maturity of {contract.TimeToMaturity} years.";
        }
    }
}

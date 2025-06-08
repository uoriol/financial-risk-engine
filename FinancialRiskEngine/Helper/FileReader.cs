using CsvHelper;
using FinancialRiskEngine.Engine.Classes.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialRiskEngine.Helper
{
    public static class FileReader
    {

        public static List<Price> GetHistoricalVolatilityIndexValues(bool returnAll = false)
        {
            using var reader = new StreamReader("Resources/VIX_History.csv");
            using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            var volatilityIndexValues = new List<Price>();
            while (csv.Read())
            {
                var price = new Price();
                price.Open = csv.GetField<double>("OPEN");
                price.Close = csv.GetField<double>("CLOSE");
                price.Date = csv.GetField<DateTime>("DATE");
                volatilityIndexValues.Add(price);
            }

            csv.Dispose();

            if (returnAll) {
                return volatilityIndexValues;
            }

            // Let's just take 1000, it is enough to model the valies and not necessary to overdo it
            return volatilityIndexValues.TakeLast(1000).ToList();
        }

        public static List<Price> GetHistoricalSP500Values(bool returnAll = false)
        {
            using var reader = new StreamReader("Resources/SP500_History.csv");
            using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            var prices = new List<Price>();
            while (csv.Read())
            {
                var price = new Price();
                price.Open = csv.GetField<double>("Open");
                price.Close = csv.GetField<double>("Close/Last");
                price.Date = csv.GetField<DateTime>("Date");
                prices.Add(price);
            }

            csv.Dispose();

            if (returnAll)
            {
                return prices;
            }

            return prices.TakeLast(1000).ToList();
        }
    }
}

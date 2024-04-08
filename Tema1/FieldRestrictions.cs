using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1
{
    public static class FieldRestrictions
    {
        public static readonly List<string> CompayNames = new List<string>()
        {
            "Apple Inc.",
            "Microsoft Corporation",
            "Alphabet Inc.",
            "Amazon.com Inc.",
            "Meta Platforms Inc.",
            "Tesla Inc.",
            "NVIDIA Corporation",
            "Berkshire Hathaway Inc.",
            "Johnson & Johnson",
            "JPMorgan Chase & Co.",
            "Walmart Inc.",
            "Visa Inc.",
            "Procter & Gamble Company",
            "Mastercard Incorporated",
            "UnitedHealth Group Incorporated",
            "Intel Corporation",
            "Home Depot Inc.",
            "Bank of America Corporation",
            "Taiwan Semiconductor Manufacturing Company Limited",
            "Verizon Communications Inc.",
            "Adobe Inc.",
            "Abbott Laboratories",
            "Coca-Cola Company",
            "Cisco Systems Inc.",
            "Pfizer Inc.",
            "Merck & Co. Inc.",
            "Walt Disney Company",
            "Salesforce.com Inc.",
            "Netflix Inc.",
            "IBM"
        };

        public static readonly (double min, double max) ValueRange = (0d, 200d);
        public static readonly (double min, double max) DropRange = (0d, 50d);
        public static readonly (double min, double max) VariationRange = (0d, 1d);
        public static readonly (DateOnly startDate, int maxDaysAdd) DateRange = (new DateOnly(2001, 12, 28), 30);

        public static double GetRandomNumber((double minimum, double maximum) range)
        {
            (double minimum, double maximum) = range;
            if(minimum == 0 && maximum == 1)
            {
                return Random.Shared.NextDouble();
            }

            return (Random.Shared.NextDouble() * (maximum - minimum)) + minimum;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1
{
    public class Publication
    {
        public string Company { get; set; }
        public double Value { get; set; }
        public double Drop { get; set; }
        public double Variation { get; set; }
        public DateOnly Date { get; set; }

        public Publication()
        {
            Company = "";
            Value = 0;
            Drop = 0;
            Variation = 0;
            Date = new DateOnly();
        }

        public static Publication CreateRandPublication()
        {
            return new Publication()
            {
                Company = FieldRestrictions.CompayNames[Random.Shared.Next(FieldRestrictions.CompayNames.Count)],
                Value = FieldRestrictions.GetRandomNumber(FieldRestrictions.ValueRange),
                Drop = FieldRestrictions.GetRandomNumber(FieldRestrictions.DropRange),
                Variation = FieldRestrictions.GetRandomNumber(FieldRestrictions.VariationRange),
                Date = FieldRestrictions.DateRange.startDate.AddDays(Random.Shared.Next(FieldRestrictions.DateRange.maxDaysAdd))
            };
        }

        public override string ToString()
        {
            var toDoubleString = (double val) => String.Format("{0:0.00}", val);
            return $"{{(company, \"{Company}\");(value, \"{toDoubleString(Value)}\");(drop, \"{toDoubleString(Drop)}\");(variation, \"{toDoubleString(Variation)}\");(date, \"{Date.ToShortDateString()}\")}}";
        }
    }
}

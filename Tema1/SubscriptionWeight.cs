namespace Tema1
{
    public enum Field { COMPANY, VALUE, DROP, VARIATION, DATE }
    public class SubscriptionWeight
    {
        public Field Attribute { get; set; }

        private double _eqWeight = 0;

        private double _weight = 0;

        public double Weight
        {
            get => _weight;
            set
            {
                if (value < 0) _weight = 0;
                else if (value > 1) _weight = 1;
                else _weight = value;
            }
        }

        // Equality operator weight
        public double EqWeight
        {
            get => _eqWeight;
            set
            {
                if (value < 0) _eqWeight = 0;
                else if (value > 1) _eqWeight = 1;
                else _eqWeight = value;
            }
        }
    }
}
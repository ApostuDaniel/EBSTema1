namespace Tema1
{
    public enum Operator
    { EQ, NEQ, LT, GT, LE, GE }

    public class SubscriptionField<T> : ISubscriptionField
    {
        public string Attribute { get; set; }
        public Operator Operator { get; set; }
        public T Value { get; set; }

        public Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        public override string ToString()
        {
            var toDoubleString = (double val) => String.Format("{0:0.00}", val);

            string result = $"({Attribute}";
            result += this.Operator switch
            {
                Operator.EQ => " = ",
                Operator.NEQ => " != ",
                Operator.LT => " < ",
                Operator.GT => " > ",
                Operator.LE => " <= ",
                Operator.GE => " >= ",
            };

            result += this.Value switch
            {
                double d => toDoubleString(d),
                DateOnly date => date.ToShortDateString(),
                string s => s,
            };

            result += ')';

            return result;
        }
    }
}
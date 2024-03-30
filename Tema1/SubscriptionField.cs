using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1
{
    public enum Operator { EQ, NEQ, LT, GT, LE, GE }
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
    }
}

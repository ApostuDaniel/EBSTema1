using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1
{
    public class Subscription
    {
        public List<ISubscriptionField> SubscriptionFields { get; set; }

        public Subscription()
        {
            SubscriptionFields = new List<ISubscriptionField>();
        }

        public Subscription(List<ISubscriptionField> subscriptionFields)
        {
            SubscriptionFields = subscriptionFields;
        }

        //Tries to add the subscription fields of another subscription
        //Returns true if the other subscription does not have a subscription field with the same Attribute, false otherwise
        public static Subscription CombineSubscriptions(Subscription source, Subscription partner)
        {
            var combination = new Subscription(source.SubscriptionFields);
            combination.SubscriptionFields.AddRange(partner.SubscriptionFields);
            
            return combination;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append('{');
            foreach (var subField in SubscriptionFields)
            {
                sb.Append(subField.ToString());
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append('}');

            return sb.ToString();
        }
    }
}

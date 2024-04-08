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
    }
}

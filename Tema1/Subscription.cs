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
    }
}

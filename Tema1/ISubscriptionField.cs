﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1
{
    public interface ISubscriptionField
    {
        public string Attribute { get; set; }
        public Operator Operator { get; set; }
    }
}

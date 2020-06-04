using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Programming_Assessment
{
    public class Payment
    {
        public String Customer { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public float Amount { get; set; }

    }
}

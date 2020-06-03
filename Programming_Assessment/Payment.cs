using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Programming_Assessment
{
    public class Payment
    {
        [JsonProperty("Customer")]
        public String Customer { get; set; }
        [JsonProperty("Year")]
        public int Year { get; set; }
        [JsonProperty("Month")]
        public int Month { get; set; }
        [JsonProperty("Amount")]
        public float Amount { get; set; }
    }
}

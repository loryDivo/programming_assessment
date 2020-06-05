﻿using Newtonsoft.Json;
using System;

namespace Programming_Assessment
{
    public class Payment
    {
        public String Customer { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        [JsonProperty("Amount")]
        private float Amount;

        public float GetAmount()
        {
            return Amount;
        }

        public void SetAmount(float iAmount)
        {
            if (Amount < 0)
            {
                throw new ArgumentException("Amount cannot be less than 0");
            }
            Amount = iAmount;
        }

        public override bool Equals(object obj)
        {
            return obj is Payment payment &&
                   Customer == payment.Customer &&
                   Year == payment.Year &&
                   Month == payment.Month &&
                   Amount == payment.Amount;
        }

        public Payment(String iCustomer, int iYear, int iMonth, float iAmount)
        {
            if (string.IsNullOrEmpty(iCustomer))
            {
                throw new ArgumentException("Customer cannot be null or empty");
            }
            if (iAmount < 0)
            {
                throw new ArgumentException("Amount cannot be less than 0");
            }
            this.Customer = iCustomer;
            this.Year = iYear;
            this.Month = iMonth;
            this.SetAmount(iAmount);
        }
        public Payment(String iCustomer, DateTime iDate, float iAmount): this(iCustomer, iDate.Year, iDate.Month, iAmount)
        {
        }
        public Payment()
        {
        }
    }
}

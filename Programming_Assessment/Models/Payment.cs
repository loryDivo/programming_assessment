using Newtonsoft.Json;
using System;

namespace Programming_Assessment
{
    public class Payment
    {
        public String Customer { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public float Amount { get; private set;}

        public override bool Equals(object obj)
        {
            return obj is Payment payment &&
                   Customer == payment.Customer &&
                   Year == payment.Year &&
                   Month == payment.Month &&
                   Amount == payment.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Customer, Year, Month, Amount);
        }
        [JsonConstructor]
        public Payment(String Customer, int Year, int Month, float Amount)
        {
            if (string.IsNullOrEmpty(Customer))
            {
                throw new ArgumentException("Customer cannot be null or empty");
            }
            if (Amount < 0)
            {
                throw new ArgumentException("Amount cannot be less than 0");
            }
            this.Customer = Customer;
            this.Year = Year;
            this.Month = Month;
            this.Amount = Amount;
        }
        public Payment(String iCustomer, DateTime iDate, float iAmount): this(iCustomer, iDate.Year, iDate.Month, iAmount)
        {
        }
    }
}

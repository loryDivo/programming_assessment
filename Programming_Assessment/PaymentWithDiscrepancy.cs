using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Programming_Assessment
{
    public class PaymentWithDiscrepancy : Payment, IComparable<PaymentWithDiscrepancy>
    {
        public float AmountDue;
        public float differenceBetweenDueAndPayed;
        public PaymentWithDiscrepancy(Payment payment)
        {
            this.Customer = payment.Customer;
            this.Month = payment.Month;
            this.Year = payment.Year;
            this.Amount = payment.Amount;
        }

        public int CompareTo(PaymentWithDiscrepancy other)
        {
            if(this.differenceBetweenDueAndPayed.CompareTo(other.differenceBetweenDueAndPayed) > 0)
            {
                return -1;
            }
            else if (this.differenceBetweenDueAndPayed.CompareTo(other.differenceBetweenDueAndPayed) < 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}

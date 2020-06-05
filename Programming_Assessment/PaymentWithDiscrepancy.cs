using System;

namespace Programming_Assessment
{
    public class PaymentWithDiscrepancy : Payment, IComparable<PaymentWithDiscrepancy>
    {
        public float AmountDue { get; set; }
        public float DifferenceBetweenDueAndPayed { get; set; }
        public PaymentWithDiscrepancy(Payment iPayment)
        {
            this.Customer = iPayment.Customer;
            this.Month = iPayment.Month;
            this.Year = iPayment.Year;
        }
        public PaymentWithDiscrepancy()
        {
        }

        public int CompareTo(PaymentWithDiscrepancy iOther)
        {
            if(this.DifferenceBetweenDueAndPayed.CompareTo(iOther.DifferenceBetweenDueAndPayed) > 0)
            {
                return -1;
            }
            else if (this.DifferenceBetweenDueAndPayed.CompareTo(iOther.DifferenceBetweenDueAndPayed) < 0)
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Programming_Assessment
{
    public class PaymentWithDiscrepancy : Payment, IComparable<PaymentWithDiscrepancy>
    {
        [JsonProperty("AmountDue")]
        private float AmountDue;
        [JsonProperty("DifferenceBetweenDueAndPayed")]
        private float DifferenceBetweenDueAndPayed;


        public PaymentWithDiscrepancy(Payment iPayment)
        {
            this.Customer = iPayment.Customer;
            this.Month = iPayment.Month;
            this.Year = iPayment.Year;
        }
        public PaymentWithDiscrepancy()
        {
        }
        public float GetDifferenceBetweenDueAndPayed()
        {
            return DifferenceBetweenDueAndPayed;
        }

        public void SetDifferenceBetweenDueAndPayed(float iDifferenceBetweenDueAndPayed)
        {
            if (iDifferenceBetweenDueAndPayed < 0)
            {
                throw new ArgumentException("Difference cannot be less than 0");
            }
            DifferenceBetweenDueAndPayed = iDifferenceBetweenDueAndPayed;
        }
        public float GetAmountDue()
        {
            return AmountDue;
        }

        public void SetAmountDue(float iAmountDue)
        {
            if (iAmountDue < 0)
            {
                throw new ArgumentException("Amount due cannot be less than 0");
            }
            AmountDue = iAmountDue;
        }

        public int CompareTo(PaymentWithDiscrepancy iOther)
        {
            if(this.GetDifferenceBetweenDueAndPayed().CompareTo(iOther.GetDifferenceBetweenDueAndPayed()) > 0)
            {
                return -1;
            }
            else if (this.GetDifferenceBetweenDueAndPayed().CompareTo(iOther.GetDifferenceBetweenDueAndPayed()) < 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    public interface IPaymentWithDiscrepancyFormatter
    {
        String AsCsv(PaymentWithDiscrepancy iPaymentWithDiscrepancy);
    }
    public sealed class PaymentWithDiscrepancyText : IPaymentWithDiscrepancyFormatter
    {
        public string AsCsv(PaymentWithDiscrepancy iPaymentWithDiscrepancy)
        {
            if (iPaymentWithDiscrepancy == null)
            {
                throw new ArgumentNullException("PaymentWithDiscrepancy is null");
            }

            return string.Format(
                "{0},{1},{2},{3},{4},{5}",
                iPaymentWithDiscrepancy.Customer,
                iPaymentWithDiscrepancy.Month.ToString(),
                iPaymentWithDiscrepancy.Year.ToString(),
                iPaymentWithDiscrepancy.GetAmount(),
                iPaymentWithDiscrepancy.GetAmountDue(),
                iPaymentWithDiscrepancy.GetDifferenceBetweenDueAndPayed());
        }
    }

    public sealed class PaymentWithDiscrepancyWriter : StreamWriter
    {
        private static readonly String BaseDirectory = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        private readonly IPaymentWithDiscrepancyFormatter PaymentWithDiscrepancyFormatter;

        public PaymentWithDiscrepancyWriter(string iPath, bool iAppend, Encoding iEncoding, IPaymentWithDiscrepancyFormatter iPaymentWithDiscrepancyFormatter) : base(System.IO.Path.Combine(BaseDirectory, iPath), iAppend, iEncoding)
        {
            if (iPaymentWithDiscrepancyFormatter == null)
            {
                throw new ArgumentNullException("studentToCsv");
            }

            this.PaymentWithDiscrepancyFormatter = iPaymentWithDiscrepancyFormatter;
        }

        public void WritePaymentWithDiscrepancy(PaymentWithDiscrepancy iPaymentWithDiscrepancy)
        {
            if (iPaymentWithDiscrepancy == null)
            {
                throw new ArgumentNullException("PaymentWithDiscrepancy cannot be null");
            }

            this.WriteLine(this.PaymentWithDiscrepancyFormatter.AsCsv(iPaymentWithDiscrepancy));
        }

        public void WritePaymentsWithDiscrepancy(IEnumerable<PaymentWithDiscrepancy> iPaymentWithDiscrepancy)
        {
            if (iPaymentWithDiscrepancy == null)
            {
                throw new ArgumentNullException("PaymentWithDiscrepancy cannot be null");
            }
            foreach (PaymentWithDiscrepancy aPaymentWithDiscrepancy in iPaymentWithDiscrepancy)
            {
                this.WritePaymentWithDiscrepancy(aPaymentWithDiscrepancy);
            }
        }
    }
}

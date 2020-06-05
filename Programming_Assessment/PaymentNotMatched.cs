using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Programming_Assessment
{
    public class PaymentNotMatched : Payment, IComparable<PaymentNotMatched>
    {
        public float AmountDue { get; set; }
        public float DifferenceBetweenDueAndPayed { get; set; }


        public PaymentNotMatched(Payment iPayment, float iAmount, float iAmountDue, float iDifferenceBetweenDueAndPayed) : base(iPayment.Customer, iPayment.Year, iPayment.Month, iAmount)
        {
            this.AmountDue = iAmountDue;
            this.DifferenceBetweenDueAndPayed = iDifferenceBetweenDueAndPayed;
        }
        public PaymentNotMatched()
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

        public int CompareTo(PaymentNotMatched iOther)
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
    public interface IPaymentNotMatchedFormatter
    {
        String AsCsv(PaymentNotMatched iPaymentNotMatched);
    }
    public sealed class PaymentNotMatchedText : IPaymentNotMatchedFormatter
    {
        public string AsCsv(PaymentNotMatched iPaymentNotMatched)
        {
            if (iPaymentNotMatched == null)
            {
                throw new ArgumentNullException("PaymentNotMatched is null");
            }

            return string.Format(
                "{0},{1},{2},{3},{4},{5}",
                iPaymentNotMatched.Customer,
                iPaymentNotMatched.Month.ToString(),
                iPaymentNotMatched.Year.ToString(),
                iPaymentNotMatched.Amount,
                iPaymentNotMatched.AmountDue,
                iPaymentNotMatched.DifferenceBetweenDueAndPayed);
        }
    }

    public sealed class PaymentNotMatchedWriter : StreamWriter
    {
        private static readonly String BaseDirectory = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        private readonly IPaymentNotMatchedFormatter PaymentNotMatchedFormatter;

        public PaymentNotMatchedWriter(string iPath, bool iAppend, Encoding iEncoding, IPaymentNotMatchedFormatter iPaymentNotMatchedFormatter) : base(System.IO.Path.Combine(BaseDirectory, iPath), iAppend, iEncoding)
        {
            if (iPaymentNotMatchedFormatter == null)
            {
                throw new ArgumentNullException("studentToCsv");
            }

            this.PaymentNotMatchedFormatter = iPaymentNotMatchedFormatter;
        }

        public void WritePaymentNotMatched(PaymentNotMatched iPaymentNotMatched)
        {
            if (iPaymentNotMatched == null)
            {
                throw new ArgumentNullException("PaymentNotMatched cannot be null");
            }

            this.WriteLine(this.PaymentNotMatchedFormatter.AsCsv(iPaymentNotMatched));
        }

        public void WritePaymentsNotMatched(IEnumerable<PaymentNotMatched> iPaymentNotMatched)
        {
            if (iPaymentNotMatched == null)
            {
                throw new ArgumentNullException("PaymentNotMatched cannot be null");
            }
            foreach (PaymentNotMatched aPaymentNotMatched in iPaymentNotMatched)
            {
                this.WritePaymentNotMatched(aPaymentNotMatched);
            }
        }
    }
}

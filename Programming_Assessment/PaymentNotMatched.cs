﻿using HtmlAgilityPack;
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
        String AsCsv(PaymentNotMatched iPaymentNotMatched, bool iAddTitleToCsv);
        String AsCsv(IEnumerable<PaymentNotMatched> iPaymentsNotMatched);
        String AsHtml(IEnumerable<PaymentNotMatched> iPaymentsNotMatched);
        String AsHtml(PaymentNotMatched iPaymentNotMatched);
        String AddTitleToCsv();
        void AddPaymentNotMatchedToHtml(HtmlDocument iPaymentNotMatchedHtml, HtmlNode iPaymentNotMatchedTable, PaymentNotMatched iPaymentNotMatched);
        void AddTitleToHtml(HtmlDocument iPaymentNotMatchedHtml, HtmlNode iPaymentNotMatchedTable);

        }
    public sealed class PaymentNotMatchedText : IPaymentNotMatchedFormatter
    {
        private static String CustomerTitle = "Customer";
        private static String YearTitle = "Year";
        private static String MonthTitle = "Month";
        private static String AmountTitle = "Amount";
        private static String AmountDueTitle = "AmountDue";
        private static String DifferenceBetweenDueAndPayedTitle = "DifferenceBetweenDueAndPayed";
        public String AddTitleToCsv()
        {
            return (CustomerTitle + "," + YearTitle + "," + MonthTitle + "," + AmountTitle + "," + AmountDueTitle + "," + DifferenceBetweenDueAndPayedTitle).ToString();
        }
        public String AsCsv(PaymentNotMatched iPaymentNotMatched, bool iAddTitleToCsv)
        {
            if (iPaymentNotMatched == null)
            {
                throw new ArgumentNullException("PaymentNotMatched is null");
            }
            StringBuilder aPaymentNotMatchedCsv = new StringBuilder();
            if (iAddTitleToCsv)
            {
                aPaymentNotMatchedCsv.AppendLine(AddTitleToCsv());
            }

            return aPaymentNotMatchedCsv.Append(string.Format(
                "{0},{1},{2},{3},{4},{5}",
                iPaymentNotMatched.Customer,
                iPaymentNotMatched.Month.ToString(),
                iPaymentNotMatched.Year.ToString(),
                iPaymentNotMatched.Amount,
                iPaymentNotMatched.AmountDue,
                iPaymentNotMatched.DifferenceBetweenDueAndPayed)).ToString();
        }
        public String AsCsv(IEnumerable<PaymentNotMatched> iPaymentsNotMatched)
        {
            if (iPaymentsNotMatched == null)
            {
                throw new ArgumentNullException("PaymentsNotMatched is null");
            }
            StringBuilder aPaymentsNotMatchedCsv = new StringBuilder();
            aPaymentsNotMatchedCsv.AppendLine(CustomerTitle + ","
                                              + YearTitle + ","
                                              + MonthTitle + ","
                                              + AmountTitle + ","
                                              + AmountDueTitle + ","
                                              + DifferenceBetweenDueAndPayedTitle);
            foreach (PaymentNotMatched aPaymentNotMatched in iPaymentsNotMatched)
            {
                aPaymentsNotMatchedCsv.AppendLine(AsCsv(aPaymentNotMatched, false));
            }
            return aPaymentsNotMatchedCsv.ToString();
        }

        public void AddTitleToHtml(HtmlDocument iPaymentNotMatchedHtml, HtmlNode iPaymentNotMatchedTable)
        {
            // Insert row with field names
            HtmlNode aPaymentNotMatchedRowName = iPaymentNotMatchedHtml.CreateElement("tr");
            iPaymentNotMatchedTable.ChildNodes.Append(aPaymentNotMatchedRowName);
            HtmlNode aPaymentNotMatchedCustomerRowName = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedCustomerRowName.InnerHtml = CustomerTitle;
            aPaymentNotMatchedRowName.ChildNodes.Append(aPaymentNotMatchedCustomerRowName);
            HtmlNode aPaymentNotMatchedYearRowName = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedYearRowName.InnerHtml = YearTitle;
            aPaymentNotMatchedRowName.ChildNodes.Append(aPaymentNotMatchedYearRowName);
            HtmlNode aPaymentNotMatchedMonthRowName = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedMonthRowName.InnerHtml = MonthTitle;
            aPaymentNotMatchedRowName.ChildNodes.Append(aPaymentNotMatchedMonthRowName);
            HtmlNode aPaymentNotMatchedAmountRowName = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedAmountRowName.InnerHtml = AmountTitle;
            aPaymentNotMatchedRowName.ChildNodes.Append(aPaymentNotMatchedAmountRowName);
            HtmlNode aPaymentNotMatchedAmountDueRowName = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedAmountDueRowName.InnerHtml = AmountDueTitle;
            aPaymentNotMatchedRowName.ChildNodes.Append(aPaymentNotMatchedAmountDueRowName);
            HtmlNode aPaymentNotMatchedDifferenceBetweenDueAndPayedRowName = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedAmountDueRowName.InnerHtml = DifferenceBetweenDueAndPayedTitle;
            aPaymentNotMatchedRowName.ChildNodes.Append(aPaymentNotMatchedDifferenceBetweenDueAndPayedRowName);
        }
        public void AddPaymentNotMatchedToHtml(HtmlDocument iPaymentNotMatchedHtml, HtmlNode iPaymentNotMatchedTable, PaymentNotMatched iPaymentNotMatched)
        {
            HtmlNode aPaymentNotMatchedRow = iPaymentNotMatchedHtml.CreateElement("tr");
            iPaymentNotMatchedTable.ChildNodes.Append(aPaymentNotMatchedRow);
            HtmlNode aPaymentNotMatchedCustomerRow = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedCustomerRow.InnerHtml = iPaymentNotMatched.Customer;
            aPaymentNotMatchedRow.ChildNodes.Append(aPaymentNotMatchedCustomerRow);
            HtmlNode aPaymentNotMatchedYearRow = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedYearRow.InnerHtml = iPaymentNotMatched.Year.ToString();
            aPaymentNotMatchedRow.ChildNodes.Append(aPaymentNotMatchedYearRow);
            HtmlNode aPaymentNotMatchedMonthRow = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedMonthRow.InnerHtml = iPaymentNotMatched.Month.ToString();
            aPaymentNotMatchedRow.ChildNodes.Append(aPaymentNotMatchedMonthRow);
            HtmlNode aPaymentNotMatchedAmountRow = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedAmountRow.InnerHtml = iPaymentNotMatched.Amount.ToString();
            aPaymentNotMatchedRow.ChildNodes.Append(aPaymentNotMatchedAmountRow);
            HtmlNode aPaymentNotMatchedAmountDueRow = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedAmountDueRow.InnerHtml = iPaymentNotMatched.AmountDue.ToString();
            aPaymentNotMatchedRow.ChildNodes.Append(aPaymentNotMatchedAmountDueRow);
            HtmlNode aPaymentNotMatchedDifferenceBetweenDueAndPayedRow = iPaymentNotMatchedHtml.CreateElement("td");
            aPaymentNotMatchedAmountDueRow.InnerHtml = iPaymentNotMatched.DifferenceBetweenDueAndPayed.ToString();
            aPaymentNotMatchedRow.ChildNodes.Append(aPaymentNotMatchedDifferenceBetweenDueAndPayedRow);
        }

        public String AsHtml(PaymentNotMatched iPaymentNotMatched)
        {
            HtmlDocument aPaymentNotMatchedHtml = new HtmlDocument();
            // Html Structure
            HtmlNode aPaymentNotMatchedHtmlDom = HtmlNode.CreateNode("<html><head></head><body></body></html>");
            // Give to HtmlDocument a DOM structure
            aPaymentNotMatchedHtml.DocumentNode.AppendChild(aPaymentNotMatchedHtmlDom);
            // Select Html Body
            HtmlNode aPaymentNotMatchedHtmlBody = aPaymentNotMatchedHtml.DocumentNode.SelectSingleNode("//body");
            HtmlNode aPaymentNotMatchedTable = aPaymentNotMatchedHtml.CreateElement("table");
            //Add Border to the table
            aPaymentNotMatchedTable.Attributes.Add("border", "1");
            AddTitleToHtml(aPaymentNotMatchedHtml, aPaymentNotMatchedHtmlDom);
            AddPaymentNotMatchedToHtml(aPaymentNotMatchedHtml, aPaymentNotMatchedHtmlDom, iPaymentNotMatched);
            return aPaymentNotMatchedHtml.DocumentNode.OuterHtml;
        }

        public String AsHtml(IEnumerable<PaymentNotMatched> iPaymentsNotMatched)
        {
            HtmlDocument aPaymentNotMatchedHtml = new HtmlDocument();
            // Html Structure
            HtmlNode aPaymentNotMatchedHtmlDom = HtmlNode.CreateNode("<html><head></head><body></body></html>");
            // Give to HtmlDocument a DOM structure
            aPaymentNotMatchedHtml.DocumentNode.AppendChild(aPaymentNotMatchedHtmlDom);
            // Select Html Body
            HtmlNode aPaymentNotMatchedHtmlBody = aPaymentNotMatchedHtml.DocumentNode.SelectSingleNode("//body");
            HtmlNode aPaymentNotMatchedTable = aPaymentNotMatchedHtml.CreateElement("table");
            //Add Border to the table
            aPaymentNotMatchedTable.Attributes.Add("border", "1");
            AddTitleToHtml(aPaymentNotMatchedHtml, aPaymentNotMatchedTable);
            // For each item add a new row inside the table and add column for each item field
            foreach (PaymentNotMatched aPaymentNotMatched in iPaymentsNotMatched)
            {
                AddPaymentNotMatchedToHtml(aPaymentNotMatchedHtml, aPaymentNotMatchedTable, aPaymentNotMatched);
            }
            // Add Table to Html Body
            aPaymentNotMatchedHtmlBody.ChildNodes.Add(aPaymentNotMatchedTable);
            return aPaymentNotMatchedHtml.DocumentNode.OuterHtml;
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

        public void WritePaymentNotMatchedToCsv(PaymentNotMatched iPaymentNotMatched, bool iAddTitleToCsv)
        {
            if (iPaymentNotMatched == null)
            {
                throw new ArgumentNullException("PaymentNotMatched cannot be null");
            }

            this.WriteLine(this.PaymentNotMatchedFormatter.AsCsv(iPaymentNotMatched, iAddTitleToCsv));
        }

        public void WritePaymentsNotMatchedToCsv(IEnumerable<PaymentNotMatched> iPaymentsNotMatched)
        {
            if (iPaymentsNotMatched == null)
            {
                throw new ArgumentNullException("PaymentNotMatched cannot be null");
            }
            this.WriteLine(this.PaymentNotMatchedFormatter.AsCsv(iPaymentsNotMatched));
        }
        public void WritePaymentsNotMatchedToHtml(IEnumerable<PaymentNotMatched> iPaymentsNotMatched)
        {
            if (iPaymentsNotMatched == null)
            {
                throw new ArgumentNullException("PaymentNotMatched cannot be null");
            }
            this.WriteLine(this.PaymentNotMatchedFormatter.AsHtml(iPaymentsNotMatched));
        }
    }
}

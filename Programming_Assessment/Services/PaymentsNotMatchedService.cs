using System;
using System.Collections.Generic;
using System.Linq;

namespace Programming_Assessment
{
    public class PaymentsNotMatchedService
    {
        private Purchases Purchases;
        private ItemPricesRoot ItemPricesRoot;
        private HashSet<Payment> PaymentsPayed;
        public PaymentsNotMatchedService(Purchases iPurchases, ItemPricesRoot iItemPricesRoot, HashSet<Payment> iPaymentsPayed)
        {
            this.Purchases = iPurchases;
            this.ItemPricesRoot = iItemPricesRoot;
            this.PaymentsPayed = iPaymentsPayed;
        }

        /// <summary>
        ///  This method calculate the payments that have no match between the Prices.xml and Purchases.Dat items
        /// </summary>
        /// <returns>
        /// <see cref="SortedSet{PaymentNotMatched}"/> items order by biggest difference between due and payed 
        /// </returns>

        public SortedSet<PaymentNotMatched> CalculatePaymentsNotMatched()
        {
            HashSet<Payment> aPaymentsDue = CalculatePaymentsDue();
            if (!aPaymentsDue.Any())
            {
                return new SortedSet<PaymentNotMatched>();
            }
            // PaymentsDue which are not existing inside PaymentsPayed or with attribute discrepancies
            HashSet<Payment> aPaymentsDueWithDiscrepancy = new HashSet<Payment>(aPaymentsDue.Where(paymentDue => !this.PaymentsPayed.Contains(paymentDue)));
            // PaymentsPayed which are not existing inside PaymentsDue or with attribute discrepancies
            HashSet<Payment> aPaymentsPayedWithDiscrepancy = new HashSet<Payment>(PaymentsPayed.Where(paymentPayed => !aPaymentsDue.Contains(paymentPayed)));

            HashSet<PaymentNotMatched> aPaymentsDueNotInPayed = new HashSet<PaymentNotMatched>(from aPaymentDueWithDiscrepancy in aPaymentsDueWithDiscrepancy
                                                              where !aPaymentsPayedWithDiscrepancy
                                                              .Any(aPaymentPayedWithDiscrepancy => aPaymentPayedWithDiscrepancy.Customer == aPaymentDueWithDiscrepancy.Customer &&
                                                                                                   aPaymentPayedWithDiscrepancy.Month == aPaymentDueWithDiscrepancy.Month &&
                                                                                                   aPaymentPayedWithDiscrepancy.Year == aPaymentDueWithDiscrepancy.Year)
                                                              select new PaymentNotMatched(
                                                                  aPaymentDueWithDiscrepancy.Customer,
                                                                  aPaymentDueWithDiscrepancy.Year,
                                                                  aPaymentDueWithDiscrepancy.Month,
                                                                  0,
                                                                  aPaymentDueWithDiscrepancy.Amount,
                                                                  (float)Math.Round(Math.Abs(0 - aPaymentDueWithDiscrepancy.Amount), 2)));

            HashSet<PaymentNotMatched> aPaymentsPayedNotInDue = new HashSet<PaymentNotMatched>(from aPaymentPayedWithDiscrepancy in aPaymentsPayedWithDiscrepancy
                                                              where !aPaymentsDueWithDiscrepancy
                                                              .Any(aPaymentDueWithDiscrepancy => aPaymentPayedWithDiscrepancy.Customer == aPaymentDueWithDiscrepancy.Customer &&
                                                                   aPaymentPayedWithDiscrepancy.Month == aPaymentDueWithDiscrepancy.Month &&
                                                                   aPaymentPayedWithDiscrepancy.Year == aPaymentDueWithDiscrepancy.Year)
                                                              select new PaymentNotMatched(
                                                                  aPaymentPayedWithDiscrepancy.Customer,
                                                                  aPaymentPayedWithDiscrepancy.Year,
                                                                  aPaymentPayedWithDiscrepancy.Month,
                                                                  aPaymentPayedWithDiscrepancy.Amount,
                                                                  0,
                                                                  (float)Math.Round(Math.Abs(0 - aPaymentPayedWithDiscrepancy.Amount), 2)));

            HashSet<PaymentNotMatched> aPaymentsInCommon = new HashSet<PaymentNotMatched>(from aPaymentDueWithDiscrepancy in aPaymentsDueWithDiscrepancy
                                                         from aPaymentPayedWithDiscrepancy in aPaymentsPayedWithDiscrepancy
                                                         where aPaymentDueWithDiscrepancy.Customer == aPaymentPayedWithDiscrepancy.Customer &&
                                                               aPaymentDueWithDiscrepancy.Month == aPaymentPayedWithDiscrepancy.Month &&
                                                               aPaymentDueWithDiscrepancy.Year == aPaymentPayedWithDiscrepancy.Year &&
                                                               Math.Abs(aPaymentDueWithDiscrepancy.Amount - aPaymentPayedWithDiscrepancy.Amount) != 0
                                                         select new PaymentNotMatched(
                                                             aPaymentPayedWithDiscrepancy.Customer,
                                                             aPaymentPayedWithDiscrepancy.Year,
                                                             aPaymentPayedWithDiscrepancy.Month,
                                                             aPaymentPayedWithDiscrepancy.Amount,
                                                             aPaymentDueWithDiscrepancy.Amount,
                                                             (float)Math.Round(Math.Abs(aPaymentPayedWithDiscrepancy.Amount - aPaymentDueWithDiscrepancy.Amount), 2)));
            // Sort all detected discrepancies
            return new SortedSet<PaymentNotMatched>(aPaymentsInCommon.Union(aPaymentsDueNotInPayed).Union(aPaymentsPayedNotInDue));
        }

        /// <summary>
        /// This method calculate all the payments due calculated from <see cref="Purchases.PurchasesList"/> and <see cref="ItemPricesRoot.ItemPrices.ItemsPriceSet"/>
        /// </summary>
        /// <returns>
        /// <see cref="HashSet{Payment}"/> built from <see cref="Purchases"/> and <see cref="ItemPricesRoot.ItemPrices.ItemsPriceSet"/>
        /// </returns>
        private HashSet<Payment> CalculatePaymentsDue()
        {
            HashSet<Payment> aPaymentsDue = new HashSet<Payment>();
            HashSet<String> aCustomerIds = new HashSet<String>(Purchases.PurchasesList.Select(aPurchase => aPurchase.Customer));
            foreach (String aCustomerId in aCustomerIds)
            {
                Dictionary<String, Purchases> aSameMonthsCustomerIdPurchases = DetectSameMonthsCustomerIdPurchases(aCustomerId);
                // For each month it is needed to calculate the total amount due
                foreach (KeyValuePair<String, Purchases> aSameMonthCustomerIdPurchases in aSameMonthsCustomerIdPurchases)
                {
                    float aCustomerIdMonthAmount = CalculateCustomerIdMonthAmount(aSameMonthCustomerIdPurchases.Value);
                    DateTime aCustomerIdDate = DateTime.ParseExact(aSameMonthCustomerIdPurchases.Key, "yyyyMM", null);
                    Payment aPaymentDue = new Payment(aCustomerId, aCustomerIdDate, aCustomerIdMonthAmount);
                    aPaymentsDue.Add(aPaymentDue);
                }
            }
            return aPaymentsDue;
        }

        /// <summary>
        /// This method calculate all the payments for a specific customerId for a specific month
        /// </summary>
        /// <param name="iPurchases">for a specific month</param>
        /// <returns>
        /// Total amount for the specific month calculated from <see cref="Purchases.PurchasesList"/> and <see cref="ItemPricesRoot.ItemPrices.ItemPricesSet"/>
        /// </returns>
        private float CalculateCustomerIdMonthAmount(Purchases iPurchases)
        {
            float aMonthAmount = 0;
            foreach (Purchase aPurchase in iPurchases.PurchasesList)
            {
                foreach (Item aItem in aPurchase.Items)
                {
                    ItemPrice aItemPrice = this.ItemPricesRoot.ItemPrices.ItemPriceSet.First(aItemPrice => aItemPrice.ItemNumber == aItem.ItemNumber);
                    aMonthAmount += aItemPrice.Price;
                }
            }
            // Consider only two decimal digits
            return (float)Math.Round(aMonthAmount, 2);
        }

        /// <summary>
        /// This method calculate all the <see cref="Purchases"/> related to a customer grouped by month
        /// </summary>
        /// <param name="iCustomerId" for whom the <see cref="Purchases"/> are grouped </param>
        /// <returns>
        /// <see cref="Dictionary{String, Purchases}"/> with key the Month and value the associated <see cref="Purchases"/>
        /// </returns>
        private Dictionary<String, Purchases> DetectSameMonthsCustomerIdPurchases(String iCustomerId)
        {
            Purchases aSameCustomerIdPurchases = new Purchases();
            aSameCustomerIdPurchases.PurchasesList = Purchases.PurchasesList.Select(aPurchase =>
            new Purchase(aPurchase.Customer,
                         aPurchase.Date,
                         aPurchase.Items))
            .Where(aPurchase => aPurchase.Customer == iCustomerId).ToList();
            
            Dictionary<String, Purchases> aSameMonthsCustomerIdPurchases = new Dictionary<String, Purchases>();

            HashSet<String> aCustomerIdDates = aSameCustomerIdPurchases.PurchasesList.Select(aPurchase => aPurchase.Date.ToString("yyyyMM")).ToHashSet();

            foreach (String aCustomerIdDate in aCustomerIdDates)
            {
                Purchases aSameMonthCustomerIdPurchase = new Purchases();
                aSameMonthCustomerIdPurchase.PurchasesList = aSameCustomerIdPurchases.PurchasesList.Select(aPurchase =>
                new Purchase(aPurchase.Customer,
                         aPurchase.Date,
                         aPurchase.Items))
                .Where(aPurchase => aPurchase.Date.ToString("yyyyMM") == aCustomerIdDate).ToList();
                aSameMonthsCustomerIdPurchases.Add(aCustomerIdDate, aSameMonthCustomerIdPurchase);
            }
            return aSameMonthsCustomerIdPurchases;
        }
    }
}

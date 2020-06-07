using System;
using System.Collections.Generic;
using System.Linq;

namespace Programming_Assessment
{
    public class PaymentsNotMatched
    {
        private Purchases Purchases;
        private ItemPricesRoot ItemPricesRoot;
        private List<Payment> PaymentsPayed;
        public PaymentsNotMatched(Purchases iPurchases, ItemPricesRoot iItemPricesRoot, List<Payment> iPaymentsPayed)
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
            List<Payment> aPaymentsDue = CalculatePaymentsDue();
            SortedSet<PaymentNotMatched> aPaymentsNotMatched = new SortedSet<PaymentNotMatched>();

            // PaymentsDue which are not existing inside PaymentsPayed or with attribute discrepancies
            List<Payment> aPaymentsDueWithDiscrepancy = aPaymentsDue.Where(paymentDue => !this.PaymentsPayed.Contains(paymentDue)).ToList();
            // PaymentsPayed which are not existing inside PaymentsDue or with attribute discrepancies
            List<Payment> aPaymentsPayedWithDiscrepancy = PaymentsPayed.Where(paymentPayed => !aPaymentsDue.Contains(paymentPayed)).ToList();

            List<PaymentNotMatched> aPaymentsDueNotInPayed = (from aPaymentDueWithDiscrepancy in aPaymentsDueWithDiscrepancy
                                                              where !aPaymentsPayedWithDiscrepancy
                                                              .Any(aPaymentPayedWithDiscrepancy => aPaymentPayedWithDiscrepancy.Customer == aPaymentDueWithDiscrepancy.Customer &&
                                                                                                   aPaymentPayedWithDiscrepancy.Month == aPaymentDueWithDiscrepancy.Month &&
                                                                                                   aPaymentPayedWithDiscrepancy.Year == aPaymentDueWithDiscrepancy.Year)
                                                              select new PaymentNotMatched()
                                                              {
                                                                  Customer = aPaymentDueWithDiscrepancy.Customer,
                                                                  Year = aPaymentDueWithDiscrepancy.Year,
                                                                  Month = aPaymentDueWithDiscrepancy.Month,
                                                                  Amount = 0,
                                                                  AmountDue = aPaymentDueWithDiscrepancy.Amount,
                                                                  DifferenceBetweenDueAndPayed = (float)Math.Round(Math.Abs(0 - aPaymentDueWithDiscrepancy.Amount), 2)
                                                              }).ToList();

            List<PaymentNotMatched> aPaymentsPayedNotInDue = (from aPaymentPayedWithDiscrepancy in aPaymentsPayedWithDiscrepancy
                                                              where !aPaymentsDueWithDiscrepancy
                                                              .Any(aPaymentDueWithDiscrepancy => aPaymentPayedWithDiscrepancy.Customer == aPaymentDueWithDiscrepancy.Customer &&
                                                                   aPaymentPayedWithDiscrepancy.Month == aPaymentDueWithDiscrepancy.Month &&
                                                                   aPaymentPayedWithDiscrepancy.Year == aPaymentDueWithDiscrepancy.Year)
                                                              select new PaymentNotMatched()
                                                              {
                                                                  Customer = aPaymentPayedWithDiscrepancy.Customer,
                                                                  Year = aPaymentPayedWithDiscrepancy.Year,
                                                                  Month = aPaymentPayedWithDiscrepancy.Month,
                                                                  Amount = aPaymentPayedWithDiscrepancy.Amount,
                                                                  AmountDue = 0,
                                                                  DifferenceBetweenDueAndPayed = (float)Math.Round(Math.Abs(0 - aPaymentPayedWithDiscrepancy.Amount), 2)
                                                              }).ToList();

            List<PaymentNotMatched> aPaymentsInCommon = (from aPaymentDueWithDiscrepancy in aPaymentsDueWithDiscrepancy
                                                         from aPaymentPayedWithDiscrepancy in aPaymentsPayedWithDiscrepancy
                                                         where aPaymentDueWithDiscrepancy.Customer == aPaymentPayedWithDiscrepancy.Customer &&
                                                               aPaymentDueWithDiscrepancy.Month == aPaymentPayedWithDiscrepancy.Month &&
                                                               aPaymentDueWithDiscrepancy.Year == aPaymentPayedWithDiscrepancy.Year &&
                                                               Math.Abs(aPaymentDueWithDiscrepancy.Amount - aPaymentPayedWithDiscrepancy.Amount) != 0
                                                         select new PaymentNotMatched()
                                                         {
                                                             Customer = aPaymentPayedWithDiscrepancy.Customer,
                                                             Year = aPaymentPayedWithDiscrepancy.Year,
                                                             Month = aPaymentPayedWithDiscrepancy.Month,
                                                             Amount = aPaymentPayedWithDiscrepancy.Amount,
                                                             AmountDue = aPaymentDueWithDiscrepancy.Amount,
                                                             DifferenceBetweenDueAndPayed = (float)Math.Round(Math.Abs(aPaymentPayedWithDiscrepancy.Amount - aPaymentDueWithDiscrepancy.Amount), 2)
                                                         })
                                                        .ToList();
            // Sort Payment that are common both to Payments Payed and Payments Due
            foreach (PaymentNotMatched aPaymentInCommon in aPaymentsInCommon)
            {
                aPaymentsNotMatched.Add(aPaymentInCommon);
            }

            // Sort Payment that are not inside the Payments Payed
            foreach (PaymentNotMatched aPaymentDueNotInPayed in aPaymentsDueNotInPayed)
            {
                aPaymentsNotMatched.Add(aPaymentDueNotInPayed);
            }
            // Sort Payment that are not inside the Payments Due
            foreach (PaymentNotMatched aPaymentPayedNotInDue in aPaymentsPayedNotInDue)
            {
                aPaymentsNotMatched.Add(aPaymentPayedNotInDue);
            }

            return aPaymentsNotMatched;
        }

        /// <summary>
        /// This method calculate all the payments due calculated from <see cref="Purchases.PurchasesList"/> and <see cref="ItemPricesRoot.ItemPrices.ItemsPriceSet"/>
        /// </summary>
        /// <returns>
        /// <see cref="List{Payment}"/> built from <see cref="Purchases"/> and <see cref="ItemPricesRoot.ItemPrices.ItemsPriceSet"/>
        /// </returns>
        private List<Payment> CalculatePaymentsDue()
        {
            List<Payment> aPaymentsDue = new List<Payment>();
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
            aSameCustomerIdPurchases.PurchasesList = Purchases.PurchasesList.Select(aPurchase => new Purchase()
            {
                Date = aPurchase.Date,
                Items = aPurchase.Items,
                Customer = aPurchase.Customer,
            }
            ).Where(aPurchase => aPurchase.Customer == iCustomerId).ToList();
            
            Dictionary<String, Purchases> aSameMonthsCustomerIdPurchases = new Dictionary<String, Purchases>();

            HashSet<String> aCustomerIdDates = aSameCustomerIdPurchases.PurchasesList.Select(aPurchase => aPurchase.Date.ToString("yyyyMM")).ToHashSet();

            foreach (String aCustomerIdDate in aCustomerIdDates)
            {
                Purchases aSameMonthCustomerIdPurchase = new Purchases();
                aSameMonthCustomerIdPurchase.PurchasesList = aSameCustomerIdPurchases.PurchasesList.Select(aPurchase => new Purchase()
                {
                    Date = aPurchase.Date,
                    Items = aPurchase.Items,
                    Customer = aPurchase.Customer,
                }
                ).Where(aPurchase => aPurchase.Date.ToString("yyyyMM") == aCustomerIdDate).ToList();
                aSameMonthsCustomerIdPurchases.Add(aCustomerIdDate, aSameMonthCustomerIdPurchase);
            }
            return aSameMonthsCustomerIdPurchases;
        }
    }
}

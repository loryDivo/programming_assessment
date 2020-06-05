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
        public SortedSet<PaymentWithDiscrepancy> CalculatePaymentsNotMatched()
        {
            List<Payment> aPaymentsDue = CalculatePaymentsDue();
            SortedSet<PaymentWithDiscrepancy> aPaymentsWithDiscrepancy = new SortedSet<PaymentWithDiscrepancy>();

            // PaymentsDue which are not existing inside PaymentsPayed or with attribute discrepancies
            List<Payment> aPaymentsDueWithDiscrepancy = aPaymentsDue.Where(paymentDue => !this.PaymentsPayed.Contains(paymentDue)).ToList();
            // PaymentsPayed which are not existing inside PaymentsDue or with attribute discrepancies
            List<Payment> aPaymentsPayedWithDiscrepancy = PaymentsPayed.Where(paymentPayed => !aPaymentsDue.Contains(paymentPayed)).ToList();

            List<Payment> aPaymentsDueNotInPayed = aPaymentsDueWithDiscrepancy
                .Where(paymentDueWithDiscrepancy => !aPaymentsPayedWithDiscrepancy
                .Any(paymentPayedWithDiscrepancy => paymentPayedWithDiscrepancy.Customer == paymentDueWithDiscrepancy.Customer &&
                                                    paymentPayedWithDiscrepancy.Month == paymentDueWithDiscrepancy.Month &&
                                                    paymentPayedWithDiscrepancy.Year == paymentDueWithDiscrepancy.Year))
                .ToList();
            List<Payment> aPaymentsPayedNotInDue = aPaymentsPayedWithDiscrepancy
                .Where(paymentPayedWithDiscrepancy => !aPaymentsDueWithDiscrepancy
                .Any(paymentDueWithDiscrepancy => paymentPayedWithDiscrepancy.Customer == paymentDueWithDiscrepancy.Customer &&
                                                  paymentPayedWithDiscrepancy.Month == paymentDueWithDiscrepancy.Month &&
                                                  paymentPayedWithDiscrepancy.Year == paymentDueWithDiscrepancy.Year))
                .ToList();

            // Detect common payments with amount differences
            foreach (Payment aPaymentDue in aPaymentsDue)
            {
                foreach (Payment aPaymentPayed in PaymentsPayed)
                {
                    if (aPaymentDue.Customer == aPaymentPayed.Customer &&
                        aPaymentDue.Month == aPaymentPayed.Month &&
                        aPaymentDue.Year == aPaymentPayed.Year &&
                        Math.Abs(aPaymentDue.GetAmount() - aPaymentPayed.GetAmount()) != 0)
                    {
                        PaymentWithDiscrepancy aPaymentNotMatched = new PaymentWithDiscrepancy(aPaymentPayed);
                        aPaymentNotMatched.AmountDue = aPaymentDue.GetAmount();
                        aPaymentNotMatched.SetAmount(aPaymentPayed.GetAmount());
                        //Consider only two decimal digits
                        aPaymentNotMatched.DifferenceBetweenDueAndPayed = (float)Math.Round(Math.Abs(aPaymentDue.GetAmount() - aPaymentPayed.GetAmount()), 2);
                        aPaymentsWithDiscrepancy.Add(aPaymentNotMatched);
                    }
                }
            }
            // Detect Payment that are not inside the Payments Payed or Payment Due and calculate the correct AmountDue and AmountPayed
            foreach (Payment aPaymentDueNotInPayed in aPaymentsDueNotInPayed)
            {
                PaymentWithDiscrepancy aPaymentNotMatched = new PaymentWithDiscrepancy(aPaymentDueNotInPayed);
                aPaymentNotMatched.AmountDue = aPaymentDueNotInPayed.GetAmount();
                aPaymentNotMatched.SetAmount(0);
                aPaymentNotMatched.DifferenceBetweenDueAndPayed = (float)Math.Round(Math.Abs(aPaymentNotMatched.GetAmount() - aPaymentNotMatched.AmountDue), 2);
                aPaymentsWithDiscrepancy.Add(aPaymentNotMatched);
            }
            foreach (Payment aPaymentPayedNotInDue in aPaymentsPayedNotInDue)
            {
                PaymentWithDiscrepancy aPaymentNotMatched = new PaymentWithDiscrepancy(aPaymentPayedNotInDue);
                aPaymentNotMatched.SetAmount(aPaymentPayedNotInDue.GetAmount());
                aPaymentNotMatched.AmountDue = 0;
                aPaymentNotMatched.DifferenceBetweenDueAndPayed = (float)Math.Round(Math.Abs(aPaymentNotMatched.GetAmount() - aPaymentNotMatched.AmountDue), 2);
                aPaymentsWithDiscrepancy.Add(aPaymentNotMatched);
            }

            return aPaymentsWithDiscrepancy;
        }

        private List<Payment> CalculatePaymentsDue()
        {
            List<Payment> aPaymentsDue = new List<Payment>();
            HashSet<String> aCustomerIds = new HashSet<String>(Purchases.PurchasesList.Select(purchase => purchase.Customer));
            foreach (String aCustomerId in aCustomerIds)
            {
                Dictionary<String, Purchases> aSameMonthsCustomerIdPurchases = DetectSameMonthCustomerIdPurchases(aCustomerId);
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

        private float CalculateCustomerIdMonthAmount(Purchases iPurchases)
        {
            float aMonthAmount = 0;
            foreach (Purchase aPurchase in iPurchases.PurchasesList)
            {
                foreach (Item aItem in aPurchase.Items)
                {
                    ItemPrice aItemPrice = this.ItemPricesRoot.ItemPrices.ItemPriceSet.First(itemPrice => itemPrice.Item == aItem.ItemNumber);
                    aMonthAmount += aItemPrice.Price;
                }
            }
            // Consider only two decimal digits
            return (float)Math.Round(aMonthAmount, 2);
        }

        private Dictionary<String, Purchases> DetectSameMonthCustomerIdPurchases(String iCustomerId)
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

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;

namespace Programming_Assessment
{
    public class PaymentsNotMatched
    {
        private Purchases purchases;
        private ItemPricesRoot itemPricesRoot;
        private List<Payment> paymentsPayed;
        public PaymentsNotMatched(Purchases purchases, ItemPricesRoot itemPricesRoot, List<Payment> paymentsPayed)
        {
            this.purchases = purchases;
            this.itemPricesRoot = itemPricesRoot;
            this.paymentsPayed = paymentsPayed;
        }
        public SortedSet<PaymentWithDiscrepancy> CalculatePaymentsNotMatched()
        {
            List<Payment> paymentsDue = CalculatePaymentsDue();
            SortedSet<PaymentWithDiscrepancy> commonPaymentsWithDiscrepancy = new SortedSet<PaymentWithDiscrepancy>();

            foreach (Payment paymentDue in paymentsDue)
            {
                foreach (Payment paymentPayed in paymentsPayed)
                {
                    if (paymentDue.Customer == paymentPayed.Customer &&
                        paymentDue.Month == paymentPayed.Month &&
                        paymentDue.Year == paymentPayed.Year &&
                        Math.Abs(paymentDue.Amount - paymentPayed.Amount) != 0)
                    {
                        PaymentWithDiscrepancy paymentNotMatched = new PaymentWithDiscrepancy(paymentPayed);
                        paymentNotMatched.AmountDue = paymentDue.Amount;
                        //Consider only two decimal digits
                        paymentNotMatched.differenceBetweenDueAndPayed = (float)Math.Round(Math.Abs(paymentDue.Amount - paymentPayed.Amount), 2);
                        commonPaymentsWithDiscrepancy.Add(paymentNotMatched);
                    }
                }
            }
            return commonPaymentsWithDiscrepancy;
        }
        private List<Payment> CalculatePaymentsDue()
        {
            List<Payment> paymentsDue = new List<Payment>();
            HashSet<String> customerIds = new HashSet<String>(purchases.purchases.Select(purchase => purchase.Customer));
            foreach (String customerId in customerIds)
            {
                Dictionary<String, Purchases> sameMonthsCustomerIdPurchases = DetectSameMonthCustomerIdPurchases(customerId);
                // For each month it is needed to calculate the total amount due
                foreach (KeyValuePair<String, Purchases> sameMonthCustomerIdPurchases in sameMonthsCustomerIdPurchases)
                {
                    float customerIdMonthAmount = CalculateCustomerIdMonthAmount(sameMonthCustomerIdPurchases.Value);
                    Payment paymentDue = new Payment();
                    DateTime customerIdDate = DateTime.ParseExact(sameMonthCustomerIdPurchases.Key, "yyyyMM", null);
                    paymentDue.Customer = customerId;
                    paymentDue.Amount = customerIdMonthAmount;
                    paymentDue.Month = customerIdDate.Month;
                    paymentDue.Year = customerIdDate.Year;
                    paymentsDue.Add(paymentDue);
                }
            }
            return paymentsDue;
        }

        private float CalculateCustomerIdMonthAmount(Purchases purchases)
        {
            float monthAmount = 0;
            foreach (Purchase purchase in purchases.purchases)
            {
                foreach (Item item in purchase.Items)
                {
                    ItemPrice itemPrice = this.itemPricesRoot.ItemPrices.ItemPriceSet.First(itemPrice => itemPrice.Item == item.ItemNumber);
                    monthAmount += itemPrice.Price;
                }
            }
            // Consider only two decimal digits
            return (float)Math.Round(monthAmount, 2);
        }

        private Dictionary<String, Purchases> DetectSameMonthCustomerIdPurchases(String customerId)
        {
            Purchases sameCustomerIdPurchases = new Purchases();
            sameCustomerIdPurchases.purchases = purchases.purchases.Select(purchase => new Purchase()
            {
                Date = purchase.Date,
                Items = purchase.Items,
                Customer = purchase.Customer,
            }
            ).Where(purchase => purchase.Customer == customerId).ToList();
            
            Dictionary<String, Purchases> sameMonthsCustomerIdPurchases = new Dictionary<String, Purchases>();

            HashSet<String> customerIdDates = sameCustomerIdPurchases.purchases.Select(purchase => purchase.Date.ToString("yyyyMM")).ToHashSet();

            foreach (String customerIdDate in customerIdDates)
            {
                Purchases sameMonthCustomerIdPurchase = new Purchases();
                sameMonthCustomerIdPurchase.purchases = sameCustomerIdPurchases.purchases.Select(purchase => new Purchase()
                {
                    Date = purchase.Date,
                    Items = purchase.Items,
                    Customer = purchase.Customer,
                }
                ).Where(purchase => purchase.Date.ToString("yyyyMM") == customerIdDate).ToList();
                sameMonthsCustomerIdPurchases.Add(customerIdDate, sameMonthCustomerIdPurchase);
            }
            return sameMonthsCustomerIdPurchases;
        }
    }
}

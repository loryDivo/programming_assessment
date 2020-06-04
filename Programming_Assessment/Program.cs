using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Programming_Assessment
{
    static class Program
    {
        public static void Main(string[] args)
        {
            XmlParser<ItemPricesRoot> xmlParser = new XmlParser<ItemPricesRoot>("data");
            xmlParser.LoadFile("Prices.xml");
            ItemPricesRoot itemPricesRoot = xmlParser.Deserialize("ItemPricesRoot");

            JsonParser<Payment> jsonParserPayment = new JsonParser<Payment>("data");
            jsonParserPayment.LoadFile("Payments.json");
            List <Payment> paymentsPayed = jsonParserPayment.Deserialize();

            PurchasesDatParser purchasesDatParser = new PurchasesDatParser("data");
            purchasesDatParser.LoadFile("Purchases.dat");
            Purchases purchases = purchasesDatParser.Deserialize();

            PaymentsNotMatched paymentsNotMatched = new PaymentsNotMatched(purchases, itemPricesRoot, paymentsPayed);

            JsonParser<PaymentWithDiscrepancy> jsonParserPaymentWithDiscrepancy = new JsonParser<PaymentWithDiscrepancy>("data");

            SortedSet<PaymentWithDiscrepancy> commonPaymentsWithDiscrepancy = paymentsNotMatched.CalculatePaymentsNotMatched();

            foreach (PaymentWithDiscrepancy commonPaymentWithDiscrepancy in commonPaymentsWithDiscrepancy)
            {
                jsonParserPaymentWithDiscrepancy.Serialize(commonPaymentsWithDiscrepancy, "PaymentsNotMatched.json");
            }
            
        }
    }
}

using System.Collections.Generic;
using System.Text;

namespace Programming_Assessment
{
    static class Program
    {
        public static void Main(string[] args)
        {
            // Deserialize needed data
            XmlParser<ItemPricesRoot> aXmlParser = new XmlParser<ItemPricesRoot>("data");
            aXmlParser.LoadFile("Prices.xml");
            ItemPricesRoot itemPricesRoot = aXmlParser.Deserialize("ItemPricesRoot");

            JsonParser<Payment> aJsonParserPayment = new JsonParser<Payment>("data");
            aJsonParserPayment.LoadFile("Payments.json");
            List <Payment> aPaymentsPayed = aJsonParserPayment.Deserialize();

            PurchasesDatParser aPurchasesDatParser = new PurchasesDatParser("data");
            aPurchasesDatParser.LoadFile("Purchases.dat");
            Purchases aPurchases = aPurchasesDatParser.Deserialize();

            PaymentsNotMatched aPaymentsNotMatched = new PaymentsNotMatched(aPurchases, itemPricesRoot, aPaymentsPayed);


            // Calculate Payments that have no matching
            SortedSet<PaymentWithDiscrepancy> aCommonPaymentsWithDiscrepancy = aPaymentsNotMatched.CalculatePaymentsNotMatched();

            // Store Payments that have no matching inside JSON file
            JsonParser<PaymentWithDiscrepancy> aJsonParserPaymentWithDiscrepancy = new JsonParser<PaymentWithDiscrepancy>("data");
            aJsonParserPaymentWithDiscrepancy.Serialize(aCommonPaymentsWithDiscrepancy, "PaymentsNotMatched.json");

            // Store Payments that have no matching inside CSV file
            PaymentWithDiscrepancyText aPaymentWithDiscrepancyText = new PaymentWithDiscrepancyText();

            using (PaymentWithDiscrepancyWriter aPaymentWithDiscrepancyWriter = new PaymentWithDiscrepancyWriter("data/PaymentsNotMatched.csv", false, Encoding.Default, aPaymentWithDiscrepancyText))
            {
                aPaymentWithDiscrepancyWriter.WritePaymentsWithDiscrepancy(aCommonPaymentsWithDiscrepancy);
            }
        }
    }
}

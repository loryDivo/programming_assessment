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
            SortedSet<PaymentNotMatched> aCommonPaymentsWithDiscrepancy = aPaymentsNotMatched.CalculatePaymentsNotMatched();

            // Store Payments that have no matching inside JSON file
            JsonParser<PaymentNotMatched> aJsonParserPaymentWithDiscrepancy = new JsonParser<PaymentNotMatched>("data");
            aJsonParserPaymentWithDiscrepancy.Serialize(aCommonPaymentsWithDiscrepancy, "PaymentsNotMatched.json");

            // Store Payments that have no matching inside CSV file
            PaymentNotMatchedText aPaymentWithDiscrepancyText = new PaymentNotMatchedText();

            using (PaymentNotMatchedWriter aPaymentWithDiscrepancyWriter = new PaymentNotMatchedWriter("data/PaymentsNotMatched.csv", false, Encoding.Default, aPaymentWithDiscrepancyText))
            {
                aPaymentWithDiscrepancyWriter.WritePaymentsNotMatched(aCommonPaymentsWithDiscrepancy);
            }
        }
    }
}

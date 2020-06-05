using System.Collections.Generic;

namespace Programming_Assessment
{
    static class Program
    {
        public static void Main(string[] args)
        {
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

            JsonParser<PaymentWithDiscrepancy> aJsonParserPaymentWithDiscrepancy = new JsonParser<PaymentWithDiscrepancy>("data");

            SortedSet<PaymentWithDiscrepancy> aCommonPaymentsWithDiscrepancy = aPaymentsNotMatched.CalculatePaymentsNotMatched();


            aJsonParserPaymentWithDiscrepancy.Serialize(aCommonPaymentsWithDiscrepancy, "PaymentsNotMatched.json");
            
        }
    }
}

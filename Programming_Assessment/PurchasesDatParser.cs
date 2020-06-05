using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Programming_Assessment
{
    class PurchasesDatParser : Parser<Purchases>
    {
        private const String CustomerId = "CUST";
        private const String DateId = "DATE";
        private const String ItemId = "ITEM";
        public PurchasesDatParser(String iPath) : base(iPath)
        {

        }
        public override void LoadFile(String iFileName)
        {
            StreamReader aStreamReader = new StreamReader(System.IO.Path.Combine(this.Path, iFileName), System.Text.Encoding.Default);
            this.MarkupString = aStreamReader.ReadToEnd().Trim();
            aStreamReader.Close();
        }

        public Purchases Deserialize()
        {
            Purchases aPurchases = new Purchases();
            String[] aSplittedByPurchases = this.MarkupString.Split(CustomerId, StringSplitOptions.RemoveEmptyEntries);
            foreach (String aSplittedByPurchase in aSplittedByPurchases)
            {
                String[] aSplittedByPurchaseFields = Regex.Split(aSplittedByPurchase, "\\s+", RegexOptions.None).Where(s => s!= String.Empty).ToArray<String>();
                Purchase aPurchase = new Purchase();
                foreach (String aSplittedByPurchaseField in aSplittedByPurchaseFields)
                {
                    if (aSplittedByPurchaseField.Contains(DateId))
                    {
                        String aDateString = aSplittedByPurchaseField.Replace(DateId, String.Empty);
                        aPurchase.Date = DateTime.ParseExact(aDateString, "ddMMyyyyHHmm", null);
                    }
                    else if (aSplittedByPurchaseField.Contains(ItemId))
                    {
                        Item aItem = new Item();
                        aItem.ItemNumber = aSplittedByPurchaseField.Replace(ItemId, String.Empty);
                        aPurchase.Items.Add(aItem);
                    }
                    else
                    {
                        aPurchase.Customer = aSplittedByPurchaseField;
                    }
                }
                aPurchases.PurchasesList.Add(aPurchase);
            }
            return aPurchases;
        }
    }
}

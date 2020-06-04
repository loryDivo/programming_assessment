using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Programming_Assessment
{
    class PurchasesDatParser : Parser<Purchases>
    {
        private static String customerId = "CUST";
        private static String dateId = "DATE";
        private static String itemId = "ITEM";
        public PurchasesDatParser(String path) : base(path)
        {

        }
        public override void LoadFile(String fileName)
        {
            StreamReader sr = new StreamReader(Path.Combine(this.path, fileName), System.Text.Encoding.Default);
            this.markupString = sr.ReadToEnd().Trim();
            sr.Close();
        }

        public Purchases Deserialize()
        {
            Purchases purchases = new Purchases();
            String[] splittedByPurchases = this.markupString.Split(customerId, StringSplitOptions.RemoveEmptyEntries);
            foreach (String splittedByPurchase in splittedByPurchases)
            {
                String[] splittedByPurchaseFields = Regex.Split(splittedByPurchase, "\\s+", RegexOptions.None).Where(s => s!= String.Empty).ToArray<String>();
                Purchase purchase = new Purchase();
                foreach (String splittedByPurchaseField in splittedByPurchaseFields)
                {
                    if (splittedByPurchaseField.Contains(dateId))
                    {
                        String dateString = splittedByPurchaseField.Replace(dateId, String.Empty);
                        purchase.Date = DateTime.ParseExact(dateString, "ddMMyyyyHHmm", null);
                    }
                    else if (splittedByPurchaseField.Contains(itemId))
                    {
                        Item item = new Item();
                        item.ItemNumber = splittedByPurchaseField.Replace(itemId, String.Empty);
                        purchase.Items.Add(item);
                    }
                    else
                    {
                        purchase.Customer = splittedByPurchaseField;
                    }
                }
                purchases.purchases.Add(purchase);
            }
            return purchases;
        }
    }
}

﻿using System.Xml.Serialization;

namespace Programming_Assessment
{
    [XmlRoot(ElementName = "ItemPricesRoot")]
    public class ItemPricesRoot
    {
        [XmlElement(ElementName = "ItemPricesList")]
        public ItemPricesList ItemPricesList { get; set; }
    }
    [XmlRoot(ElementName = "ItemPricesList")]
    public class ItemPricesList
    {
        [XmlElement(ElementName = "ItemPrice")]
        public ItemPrice[] ItemPrice { get; set; }
    }
    [XmlRoot(ElementName = "ItemPrice")]
    public class ItemPrice
    {
        [XmlElement(ElementName = "Item")]
        public string Item { get; set; }
        [XmlElement(ElementName = "Price")]
        public string Price { get; set; }
    }
}

using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Programming_Assessment
{
    public class XmlParser<T> : Parser<T>
    {

        public XmlParser(String iPath) : base(iPath)
        {
        }
        public override void LoadFile(String iFileName)
        {
            // load the file using;
            XDocument aXmlDocument = XDocument.Load(System.IO.Path.Combine(this.Path, iFileName));
            // convert the xml into string
            this.MarkupString = aXmlDocument.ToString();
        }
        public T Deserialize(string iRootAttribute)
        {
            XmlSerializer aSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(iRootAttribute));
            StringReader aStringReader = new StringReader(this.MarkupString);
            T ADeserializedObject = (T)aSerializer.Deserialize(aStringReader);
            aStringReader.Close();
            return ADeserializedObject;
        }
    }
}

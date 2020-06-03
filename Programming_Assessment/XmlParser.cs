using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Programming_Assessment
{
    public class XmlParser<T> : Parser<T>
    {

        public XmlParser(String path) : base(path)
        {
        }
        public override void LoadFile()
        {
            // load the file using;
            var XmlDocument = XDocument.Load(path);
            // convert the xml into string
            this.markupString = XmlDocument.ToString();
        }
        public T Deserialize(string rootAttribute)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootAttribute));
            StringReader stringReader = new StringReader(markupString);
            T deserializedObject = (T)serializer.Deserialize(stringReader);
            stringReader.Close();
            return deserializedObject;
        }
    }
}

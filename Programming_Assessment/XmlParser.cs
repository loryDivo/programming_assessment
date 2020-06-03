using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Programming_Assessment
{
    public class XmlParser<T>
    {
        private String path;
        private String xmlString;
        private String baseDirectory = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        public XmlParser(String path)
        {
            this.path = Path.Combine(baseDirectory, @path);
        }
        public void LoadFile()
        {
            // load the file using;
            var XmlDocument = XDocument.Load(path);
            // convert the xml into string
            xmlString = XmlDocument.ToString();
        }
        public T Deserialize(string rootAttribute)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootAttribute));
            StringReader stringReader = new StringReader(xmlString);
            T deserializedObject = (T)serializer.Deserialize(stringReader);
            return deserializedObject;
        }
    }
}

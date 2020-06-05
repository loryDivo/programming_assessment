using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Programming_Assessment
{
    public class JsonParser <T> : Parser<T>
    {
        public JsonParser(String iPath) : base(iPath)
        {
        }

        public override void LoadFile(String iFileName)
        {
            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(this.Path, iFileName)))
            {
                this.MarkupString = sr.ReadToEnd();
            }
        }

        public List<T> Deserialize()
        {
            List<T> aDeserializedObject = JsonConvert.DeserializeObject<List<T>>(this.MarkupString);
            return aDeserializedObject;
        }

        public void Serialize(SortedSet<T> iObjectToSerialize, String iFileName)
        {
            String aSerializedJson = JsonConvert.SerializeObject(iObjectToSerialize, Formatting.Indented);
            using (StreamWriter aWriter = new StreamWriter(System.IO.Path.Combine(this.Path, iFileName)))
            {
                aWriter.WriteLine(aSerializedJson);
            }
        }
    }
}

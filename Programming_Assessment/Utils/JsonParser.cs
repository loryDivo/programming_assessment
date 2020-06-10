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

        public IEnumerable<T> Deserialize()
        {
            IEnumerable<T> aDeserializedObject = JsonConvert.DeserializeObject<IEnumerable<T>>(this.MarkupString);
            return aDeserializedObject;
        }
        /// <summary>
        /// This method serialize <see cref="IEnumerable{T}"/> inside a specific file
        /// </summary>
        /// <param name="iObjectToSerialize"> <see cref="IEnumerable{T}"/ to serialize></param>
        /// <param name="iFileName"> the name of the file </param>
        public void Serialize(IEnumerable<T> iObjectToSerialize, String iFileName)
        {
            String aSerializedJson = JsonConvert.SerializeObject(iObjectToSerialize, Formatting.Indented);
            using (StreamWriter aWriter = new StreamWriter(System.IO.Path.Combine(this.Path, iFileName)))
            {
                aWriter.WriteLine(aSerializedJson);
            }
        }
    }
}

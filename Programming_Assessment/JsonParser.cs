using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Programming_Assessment
{
    public class JsonParser <T> : Parser<T>
    {
        public JsonParser(String path) : base(path)
        {
        }

        public override void LoadFile(String fileName)
        {
            StreamReader sr = new StreamReader(Path.Combine(this.path, fileName));
            this.markupString = sr.ReadToEnd();
            sr.Close();
        }

        public List<T> Deserialize()
        {
            List<T> deserializedObject = JsonConvert.DeserializeObject<List<T>>(this.markupString);
            return deserializedObject;
        }

        public void Serialize(SortedSet<T> objectToSerialize, String fileName)
        {
            String serializedJson = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(Path.Combine(this.path, fileName)))
            {
                writer.WriteLine(serializedJson);
            }
        }
    }
}

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

        public override void LoadFile()
        {
            StreamReader sr = new StreamReader(this.path);
            this.markupString = sr.ReadToEnd();
            sr.Close();
        }

        public List<T> Deserialize()
        {
            List<T> deserializedObject = JsonConvert.DeserializeObject<List<T>>(this.markupString);
            return deserializedObject;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monitor.Windows.Server
{
    public class CounterProvider
    {
        public static List<Counter> Load()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + "counter.json";

            using (StreamReader sr = new StreamReader(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonReader reader = new JsonTextReader(sr);
                var jObject = (JObject)serializer.Deserialize(reader);
                var configList = JsonConvert.DeserializeObject<List<Counter>>(jObject["config"].ToString());
                return configList;
            }
        }
    }
}

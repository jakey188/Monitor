using Newtonsoft.Json;

namespace Monitor.Web
{
    public class AjaxResponse
    {
        [JsonProperty("s")]
        public int S { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public class AjaxResponse<T>
    {
        [JsonProperty("s")]
        public int S { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class AjaxPageResponse 
    {
        public int S { get; set; }

        public string Msg { get; set; }

        public int PgIndex { get; set; }

        public int PgSize { get; set; }

        public int Count { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public class AjaxPageResponse<T>
    {
        public int S { get; set; }

        public string Msg { get; set; }

        public int PgIndex { get; set; }

        public int PgSize { get; set; }

        public int Count { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}

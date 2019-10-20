using System.Collections.Generic;

namespace SampleMvc
{
    public class WebRequest
    {
        public HttpMethod Method { get; set; }

        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();

        public string Body { get; set; }

        public List<QueryParameter> Parameters { get; private set; } = new List<QueryParameter>();

        public string Raw { get; private set; }

        public WebRequest(string rawRequest)
        {
            Raw = rawRequest;
        }
    }
}

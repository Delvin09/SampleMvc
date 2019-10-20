namespace SampleMvc
{
    public class WebContext
    {
        public WebRequest Request { get; private set; }

        public WebResponse Response { get; private set; }

        public WebContext(string rawRequest)
        {
            Request = new WebRequest(rawRequest);
            Response = new WebResponse();
        }
    }
}

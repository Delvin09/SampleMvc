namespace SampleMvc
{
    class ErrorHandler : IHandler
    {
        public bool Handle(WebContext context)
        {
            context.Response.Content = "Bad request";
            return true;
        }
    }
}

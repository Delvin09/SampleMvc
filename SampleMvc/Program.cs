namespace SampleMvc
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new WebServer();
            server.Add(new RequestHadler())
                .Add(new StaticHandler())
                .Add(new ControllerHandler())
                .Add(new ErrorHandler());

            server.Start();
        }
    }
}

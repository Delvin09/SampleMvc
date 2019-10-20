namespace SampleMvc
{
    public interface IController
    {
        bool ProcessRequest(WebContext context);
    }
}

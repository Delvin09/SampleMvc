namespace SampleMvc
{
    public interface IHandler {
        bool Handle(WebContext context);
    }
}

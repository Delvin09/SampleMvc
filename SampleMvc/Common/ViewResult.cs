namespace SampleMvc.Controllers
{
    public class ViewResult
    {
        public object Model { get; }
        public ViewResult(object model)
        {
            Model = model;
        }
    }
}

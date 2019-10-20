namespace SampleMvc
{
    public class WebResponse
    {
        public string Content { get; set; }

        public override string ToString()
        {
            return Content;
        }
    }
}

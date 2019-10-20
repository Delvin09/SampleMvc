namespace SampleMvc
{
    public class QueryParameter
    {
        public string Key { get; }
        public string Value { get; }
        public QueryParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}

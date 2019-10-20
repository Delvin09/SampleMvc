using System;
using System.Linq;

namespace SampleMvc
{
    class RequestHadler : IHandler
    {
        public bool Handle(WebContext context)
        {
            var lines = context.Request.Raw.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries); ;
            if (!lines.Any())
                return false;

            var queryLines = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            context.Request.Method = Enum.TryParse(queryLines[0], true, out HttpMethod method) ? method : HttpMethod.None;

            ParseParams(context, queryLines[1]);
            ParseHeaders(context, lines);
            ParseBody(context, lines);

            return false;
        }

        private void ParseBody(WebContext context, string[] lines)
        {
            foreach (var line in lines.SkipWhile(x => !string.IsNullOrEmpty(x)))
            {
                context.Request.Body += line;
            }
        }

        private void ParseHeaders(WebContext context, string[] lines)
        {
            foreach (var h in lines.Skip(1).TakeWhile(x => !string.IsNullOrEmpty(x)).Select(x => x.Split(':')))
                context.Request.Headers.Add(h.First(), h.Last());
        }

        private void ParseParams(WebContext context, string queryString)
        {
            var parameters = queryString.Split('/', '?', StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in parameters.Where(x => !x.Contains('=')))
            {
                context.Request.Parameters.Add(new QueryParameter(item, string.Empty));
            }
            var withValues = parameters.FirstOrDefault(x => x.Contains('='));
            if (!string.IsNullOrEmpty(withValues))
            {
                foreach(var p in withValues.Split('&'))
                {
                    var pair = p.Split('=');
                    context.Request.Parameters.Add(new QueryParameter(pair[0], pair[1]));
                }
            }
        }
    }
}

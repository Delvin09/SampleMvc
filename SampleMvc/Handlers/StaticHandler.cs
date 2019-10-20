using System;
using System.IO;

namespace SampleMvc
{
    class StaticHandler : IHandler
    {
        public bool Handle(WebContext context)
        {
            var p = context.Request.Parameters;
            if (p.Count == 1 && string.IsNullOrEmpty(p[0].Value) && !string.IsNullOrEmpty(p[0].Key))
            {
                var ext = Path.GetExtension(p[0].Key);
                if (!string.IsNullOrEmpty(ext)
                    && ext.Equals(".html", StringComparison.OrdinalIgnoreCase)
                    && File.Exists(@"wwwroot\" + p[0].Key))
                {
                    context.Response.Content = File.ReadAllText(@"wwwroot\" + p[0].Key);
                    return true;
                }
            }
            return false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SampleMvc.Controllers
{
    public class ControllerBase : IController
    {
        public virtual bool ProcessRequest(WebContext context)
        {
            var methods = GetType().GetMethods();
            var parameters = context.Request.Parameters.Skip(1).ToList();
            ViewResult result = null;
            MethodInfo action = null;

            action = methods.FirstOrDefault(m => m.Name.Equals(!parameters.Any() ? "Index" : parameters.First().Key, StringComparison.OrdinalIgnoreCase));
            if (action != null)
            {
                result = InvokeAction(action, context, parameters);
            }

            if (action != null && result != null)
            {
                context.Response.Content = GetResult(result, action);
                return true;
            }
            return false;
        }

        private ViewResult InvokeAction(MethodInfo action, WebContext context, List<QueryParameter> parameters)
        {
            try
            {
                return (ViewResult)action.Invoke(this, new object[] { context });
            }
            catch
            {
                return null;
            }
        }

        private string GetResult(ViewResult result, MethodInfo action)
        {
            var controllerName = GetType().Name;
            var resources = GetType().Assembly.GetManifestResourceNames()
                .FirstOrDefault(r => r.Contains(action.Name, StringComparison.OrdinalIgnoreCase)
                    && r.Contains(controllerName.Substring(0, controllerName.IndexOf("Controller")), StringComparison.OrdinalIgnoreCase));

            if (resources == null)
                return null;

            var stream = GetType().Assembly.GetManifestResourceStream(resources);
            var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();

            if (result.Model is IEnumerable)
            {
                int startLoop = text.IndexOf("{for}");
                int endLoop = text.IndexOf("{endfor}");
                if (startLoop >= 0)
                {
                    var pattern = text.Substring(startLoop + 5, endLoop - startLoop - 5);
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var item in (IEnumerable)result.Model)
                    {
                        stringBuilder.AppendLine(Format(item, pattern));
                    }
                    text = text.Replace(text.Substring(startLoop, endLoop + 8 - startLoop), stringBuilder.ToString());
                }
            }
            else
            {
                text = Format(result.Model, text);
            }
            return text;
        }

        private string Format(object item, string pattern)
        {
            var props = item.GetType().GetProperties().Select(x => x.Name);
            foreach (var p in props)
            {
                if (pattern.IndexOf("{" + p + "}", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var value = item.GetType().GetProperties()
                        .First(x => x.Name.Equals(p, StringComparison.OrdinalIgnoreCase)).GetValue(item);
                    pattern = pattern.Replace("{" + p + "}", value.ToString(), StringComparison.OrdinalIgnoreCase);
                }
            }
            return pattern;
        }
    }
}

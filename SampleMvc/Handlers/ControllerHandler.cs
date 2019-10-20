using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleMvc
{
    class ControllerHandler : IHandler
    {
        private readonly Dictionary<string, Type> _controllers = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Type, IController> _buffer = new Dictionary<Type, IController>();

        public ControllerHandler()
        {
            var controllers = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(x => x.GetTypes()
                                                    .Where(t => t.GetInterfaces().Contains(typeof(IController))));
            _controllers = controllers.ToDictionary(k => NormalizeControllerName(k.Name), v => v, StringComparer.OrdinalIgnoreCase);
        }

        private string NormalizeControllerName(string name)
        {
            var index = name.IndexOf("Controller");
            if (index > 0)
            {
                return name.Substring(0, index);
            }
            return name;
        }

        public bool Handle(WebContext context)
        {
            var controllerName = context.Request.Parameters.FirstOrDefault();
            if (controllerName != null && !string.IsNullOrEmpty(controllerName.Key) && string.IsNullOrEmpty(controllerName.Value)
                && _controllers.TryGetValue(controllerName.Key, out Type controllerType))
            {
                if (_buffer.TryGetValue(controllerType, out IController controller))
                    return controller.ProcessRequest(context);

                controller = (IController)Activator.CreateInstance(controllerType);
                _buffer[controllerType] = controller;
                return controller.ProcessRequest(context);
            }
            return false;
        }
    }
}

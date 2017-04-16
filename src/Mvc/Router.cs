using System;
using System.Collections.Generic;
using System.Reflection;
using Mvc.Attributes;
using PHttp;

namespace Mvc
{
    public class Router
    {
        private readonly Dictionary<string, Type> _methodAttributes
            = new Dictionary<string, Type>
            {
                { "GET", typeof(HttpGET) },
                { "POST", typeof(HttpPOST) },
                { "PUT", typeof(HttpPUT) },
                { "DELETE", typeof(HttpDELETE) }
            };

        public Router(string path)
        {
            ControllerName = null;
            ActionName = null;
            Url = new Dictionary<int, string>();

            string[] pathArray = path.Split('/');
            int key = 0;

            if (pathArray.Length >= 1)
                ControllerName = pathArray[0];
            if (pathArray.Length >= 2)
                ActionName = pathArray[1];
            if (pathArray.Length >= 3)
            {
                for (int i = 2; i < pathArray.Length; i++)
                    if (!pathArray[i].Equals(""))
                        Url.Add(++key, pathArray[i]);
            }
        }

        public ActionResult CallAction(Type currentType, object request, object context)
        {
            return CallAction(currentType, request, context, null);
        }

        public ActionResult CallAction(Type currentType, object request, object context, object aux)
        {
            var assembly = currentType.Assembly;
            Type type = null;

            if (ControllerName == null)
                return null;

            foreach (Type t in assembly.GetTypes())
            {
                if (t.Name.ToLower().Equals(ControllerName.ToLower() + "controller"))
                {
                    type = t;
                    break;
                }
            }

            if (type == null)
                return new Controller().NotFoundResult("Controller not Found.");

            var instance = Activator.CreateInstance(type);
            Console.WriteLine("   + Controller: {0}", instance.GetType().ToString());
            ((Controller)instance).Request = request;
            ((Controller)instance).Context = context;
            ((Controller)instance).Aux = null;
            if (aux != null)
                ((Controller)instance).Aux = aux;


            if (ActionName == null)
                return new Controller().NotFoundResult("No action specified.");

            Type httpMethodType;
            MethodInfo[] methodInfos = instance.GetType().GetMethods();

            foreach (var m in methodInfos)
            {
                _methodAttributes.TryGetValue(((HttpRequest)request).HttpMethod, out httpMethodType);
                //if (m.GetCustomAttributes(typeof(HttpGET), true).Length > 0)
                //    Console.WriteLine("Method: " + m.Name);
                if (m.Name.ToLower().Equals(ActionName.ToLower()))//m.GetCustomAttributes(_methodAttributes., true))
                {
                    if (m.GetCustomAttributes(httpMethodType, true).Length > 0)
                    {
                        Console.WriteLine("   + Action: [{0}] {1}", httpMethodType.Name, m.Name);
                        return (ActionResult)m.Invoke(instance, null);
                    }
                }
            }

            return new Controller().NotFoundResult("Action not Found.");
        }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public Dictionary<int, string> Url { get; set; }
    }
}

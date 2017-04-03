using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mvc
{
    public class Router
    {
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

        public ActionResult CallAction(Type currentType)
        {
            var assembly = currentType.Assembly;
            Type type = null;

            if (ControllerName == null)
                return null;

            foreach (Type t in assembly.GetTypes())
            {
                if (t.Name.ToLower().Equals(ControllerName.ToLower() + "controller"))
                    type = t;
            }

            if (type == null)
                return new Controller().NotFound("Controller not Found.");

            var instance = Activator.CreateInstance(type);
            Console.WriteLine("Controller: " + instance.GetType().ToString());

            if (ActionName == null)
                return new Controller().NotFound("No action specified.");

            MethodInfo[] methodInfos = instance.GetType().GetMethods();
            foreach (var m in methodInfos)
            {
                Console.WriteLine("Method: " + m.Name);
                if (m.Name.ToLower().Equals(ActionName.ToLower()))
                {
                    return (ActionResult)m.Invoke(instance, null);
                }
            }

            return new Controller().NotFound("Action not Found."); ;
        }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public Dictionary<int, string> Url { get; set; }
    }
}

using System;
using System.Reflection;

namespace Mvc
{
    public class Router
    {
        private string _controller = null;
        private string _action = null;
        private string _param = null;

        public Router(string path)
        {
            string[] pathArray = path.Split('/');

            if (pathArray.Length >= 1)
                _controller = pathArray[0];
            if (pathArray.Length >= 2)
                _action = pathArray[1];
            if (pathArray.Length >= 3)
            {
                _param = "";
                for (int i = 2; i < pathArray.Length; i++)
                    _param += pathArray[i] + "/";
            }
            if(_controller == null)
            {
                Console.WriteLine("_controller is Null");
            }
            if (_controller.Equals(""))
            {
                Console.WriteLine("_controller is Empty");
            }
        }

        public ActionResult CallAction(Type currentType)
        {
            var assembly = currentType.Assembly;
            Type type = null;

            if (_controller == null)
                return null;

            foreach (Type t in assembly.GetTypes())
            {
                if (t.Name.ToLower().Equals(_controller.ToLower() + "controller"))
                    type = t;
            }

            if (type == null)
                return Controller.NotFound("Controller not Found.");

            var instance = Activator.CreateInstance(type);
            Console.WriteLine("Controller: " + instance.GetType().ToString());

            if (_action == null)
                return Controller.NotFound("No action specified.");

            MethodInfo[] methodInfos = instance.GetType().GetMethods();
            foreach (var m in methodInfos)
            {
                Console.WriteLine("Method: " + m.Name);
                if (m.Name.ToLower().Equals(_action.ToLower()))
                {
                    return (ActionResult) m.Invoke(instance, null);
                }
            }

            return Controller.NotFound("Action not Found."); ;
        }
    }
}

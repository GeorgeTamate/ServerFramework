using System;
using PHttp.Application;

namespace Mvc
{
    public class Application : IPHttpApplication
    {
        string virtualPath = "";

        public virtual void Start()
        {
            Console.WriteLine("## Reflection Start!");
        }

        public virtual object ExecuteAction(string action)
        {
            Console.WriteLine("## Reflection ExecuteAction!");
            return "success";
        }

        public event ApplicationStartMethod applicationStartMethod;
        public event PreApplicationStartMethod preApplicationStartMethod;

        public string Name
        {
            get { return virtualPath; }
            set { virtualPath = value; }
        }
    }
}

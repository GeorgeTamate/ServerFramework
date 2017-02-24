using System;
using PHttp.Application;

namespace Mvc
{
    public class Application : IPHttpApplication
    {
        public void Start()
        {
            Console.WriteLine("## Reflection Start!");
        }

        public void ExecuteAction()
        {
            Console.WriteLine("## Reflection ExecuteAction!");
        }

        public event ApplicationStartMethod applicationStartMethod;
        public event PreApplicationStartMethod preApplicationStartMethod;

        public string Name
        {
            get { return ToString(); }
            set { Console.WriteLine("Application.Name: set yet to be implemented."); }
        }
    }
}

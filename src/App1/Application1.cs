using System;
using PHttp.Application;

namespace App1
{
    public class Application1 : IPHttpApplication
    {
        public void Start()
        {
            Console.WriteLine("-- ## Reflection Start!");
        }

        public string ExecuteAction()
        {
            Console.WriteLine("-- ## {0} Reflection ExecuteAction!", ToString());
            //HomeController.Index();

            return HomeController.Index();
        }

        public event ApplicationStartMethod applicationStartMethod;
        public event PreApplicationStartMethod preApplicationStartMethod;

        public string Name
        {
            get { return "app"; }
            set { Console.WriteLine(" ## ** Application1.Name: set yet to be implemented."); }
        }
    }
}

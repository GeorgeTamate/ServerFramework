using System;
using Mvc;

namespace App1
{
    public class Application1 : Application
    {
        //public void Start()
        //{
        //    Console.WriteLine("-- ## Reflection Start!");
        //}

        public override object ExecuteAction(string action)
        {
            Console.WriteLine("-- ## {0} Reflection ExecuteAction!", ToString());
            return HomeController.Index();
        }

        //public string Name
        //{
        //    get { return "app"; }
        //    set { Console.WriteLine(" ## ** Application1.Name: set yet to be implemented."); }
        //}
    }
}

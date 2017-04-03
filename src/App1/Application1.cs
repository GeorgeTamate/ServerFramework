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

        public override object ExecuteAction(string path)
        {
            Console.WriteLine("-- ## {0} Reflection ExecuteAction!", ToString());
            if (path == null || path.Equals(""))
                return HomeController.Index();
            
            Console.WriteLine(path);

            var router = new Router(path);
            var result = router.CallAction(GetType());

            if (result == null)
                return HomeController.Index();

            return result;
        }

        //public string Name
        //{
        //    get { return "app"; }
        //    set { Console.WriteLine(" ## ** Application1.Name: set yet to be implemented."); }
        //}
    }
}

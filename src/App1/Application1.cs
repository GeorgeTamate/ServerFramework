﻿using System;
using Mvc;

namespace App1
{
    public class Application1 : Application
    {
        //public void Start()
        //{
        //    Console.WriteLine("-- ## Reflection Start!");
        //}

        public override object ExecuteAction(object request, object context)
        {
            string path = ParsePath(request);

            Console.WriteLine("-- ## {0} Reflection ExecuteAction!", ToString());
            if (path == null || path.Equals(""))
                return new HomeController().Index();

            Console.WriteLine(path);

            var router = new Router(path);
            var result = router.CallAction(GetType(), request);

            if (result == null)
                return new HomeController().Index();

            //test
            var homeController = new HomeController();
            homeController.Context = context;
            homeController.Request = request;

            return result;
        }

        //public string Name
        //{
        //    get { return "app"; }
        //    set { Console.WriteLine(" ## ** Application1.Name: set yet to be implemented."); }
        //}
    }
}

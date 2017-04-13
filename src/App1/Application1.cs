using System;
using Mvc;

namespace App1
{
    public class Application1 : Application
    {
        public override object ExecuteAction(object request, object context)
        {
            string path = ParsePath(request);

            Console.WriteLine("-- ## {0} Reflection ExecuteAction!", ToString());
            if (path == null || path.Equals(""))
                return new HomeController().Index();

            Console.WriteLine("   + Path: {0}", path);

            var router = new Router(path);
            var result = router.CallAction(GetType(), request, context);

            if (result == null)
                return new HomeController().Index();

            //test
            var homeController = new HomeController();
            homeController.Context = context;
            homeController.Request = request;

            return result;
        }
    }
}

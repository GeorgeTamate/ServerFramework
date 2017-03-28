using Mvc;
using System.IO;

namespace App1
{
    public class HomeController : Controller
    {
        public static string Index()
        {
            var model = new
            {
                Names = ""
            };

            string view = View(
                Directory.GetCurrentDirectory() + "/../../../../view/layout.html",
                Directory.GetCurrentDirectory() + "/../../../../view/partial.html",
                model);
            return "home.html/200/OK/;" + view;
        }
    }
}

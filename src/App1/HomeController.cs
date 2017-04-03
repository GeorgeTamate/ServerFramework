using Mvc;
using System.IO;

namespace App1
{
    public class HomeController : Controller
    {
        [AttributeGET]
        public static ActionResult Index()
        {
            var model = new
            {
                Names = ""
            };

            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/partial.html";

            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, model);
            result.ContentType = "text/html";
            result.StatusCode = 200;
            result.StatusDescription = "OK";

            return result;
        }

        [AttributeGET]
        public ActionResult About()
        {
            var model = new object();
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/about.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, model);
            result.ContentType = "text/html";
            result.StatusCode = 200;
            result.StatusDescription = "OK";

            return result;

        }
    }
}

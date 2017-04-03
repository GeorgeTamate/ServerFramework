using Mvc;
using Mvc.Attributes;
using System.IO;

namespace App1
{
    public class HomeController : Controller
    {
        [HttpGET]
        public ActionResult Index()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/partial.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());
            result.ContentType = "text/html";
            result.StatusCode = 200;
            result.StatusDescription = "OK";

            return result;
        }

        [HttpGET]
        public ActionResult About()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/about.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());
            result.ContentType = "text/html";
            result.StatusCode = 200;
            result.StatusDescription = "OK";

            return result;

        }
    }
}

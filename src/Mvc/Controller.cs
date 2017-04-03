using PHttp;
using System.Collections.Generic;
using System.IO;

namespace Mvc
{
    public class Controller
    {
        private HttpRequest _request;
        private HttpContext _context;

        public string View(string layoutRoute, string viewRoute, object model, dynamic viewData = null)
        {
            var layout = File.ReadAllText(layoutRoute);
            var partial = File.ReadAllText(viewRoute);

            var partialTemplate = HandlebarsDotNet.Handlebars.Compile(new StringReader(partial));
            HandlebarsDotNet.Handlebars.RegisterTemplate("body", partialTemplate);

            var template = HandlebarsDotNet.Handlebars.Compile(layout);

            // Model wrapper
            var data = new { Model = model, ViewData = viewData };

            // Transform template into valid HTML
            return template(data);
        }

        public string NotFoundView(string layoutRoute, string message)
        {
            var layout = File.ReadAllText(layoutRoute);
            var partial = "<h1>404: Not Found</h1><br><p>{{Model.Msg}}</p>";
            var model = new { Msg = message };

            var partialTemplate = HandlebarsDotNet.Handlebars.Compile(new StringReader(partial));
            HandlebarsDotNet.Handlebars.RegisterTemplate("body", partialTemplate);

            var template = HandlebarsDotNet.Handlebars.Compile(layout);

            // Model wrapper
            var data = new { Model = model };

            // Transform template into valid HTML
            return template(data);
        }

        public ActionResult NotFound(string msg)
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";

            var result = new ActionResult();
            result.Content = NotFoundView(layoutPath, msg);
            result.ContentType = "text/html";
            result.StatusCode = 404;
            result.StatusDescription = "Not Found";

            return result;
        }

        public object Request
        {
            get { return _request; }
            set { _request = (HttpRequest)value; }
        }

        public object Context
        {
            get { return _context; }
            set { _context = (HttpContext)value; }
        }

        public Router Route { get; set; }

        public Dictionary<string, string> Session { get; set; }

        public Dictionary<string, string> User { get; set; }
    }
}

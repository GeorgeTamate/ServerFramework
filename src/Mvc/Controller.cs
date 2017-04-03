using System.IO;

namespace Mvc
{
    public class Controller
    {
        public static string View(string layoutRoute, string viewRoute, object model, dynamic viewData = null)
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

        public static string NotFoundView(string layoutRoute, string message)
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

        public static ActionResult NotFound(string msg)
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";

            var result = new ActionResult();
            result.Content = NotFoundView(layoutPath, msg);
            result.ContentType = "text/html";
            result.StatusCode = 404;
            result.StatusDescription = "Not Found";

            return result;
        }
    }
}

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
    }
}

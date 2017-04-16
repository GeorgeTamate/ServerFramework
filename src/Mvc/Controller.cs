using PHttp;
using System;
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

        public string MessageView(string layoutRoute, string partial, string message)
        {
            var layout = File.ReadAllText(layoutRoute);
            //var partial = ;
            var model = new { Msg = message };

            var partialTemplate = HandlebarsDotNet.Handlebars.Compile(new StringReader(partial));
            HandlebarsDotNet.Handlebars.RegisterTemplate("body", partialTemplate);

            var template = HandlebarsDotNet.Handlebars.Compile(layout);

            // Model wrapper
            var data = new { Model = model };

            // Transform template into valid HTML
            return template(data);
        }

        public ActionResult NotFoundResult(string msg)
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";

            var result = new ActionResult(404, "Not Found");
            result.Content = MessageView(layoutPath, "<h1>404: Not Found</h1><br><p>{{Model.Msg}}</p>", msg);

            return result;
        }

        public ActionResult UnauthorizedResult(string msg)
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";

            var result = new ActionResult(401, "Unauthorized");
            result.Content = MessageView(layoutPath, "<h1>401: Unauthorized</h1><br><p>{{Model.Msg}}</p>", msg);

            return result;
        }

        protected Dictionary<string, string> PostParams()
        {
            var parameters = new Dictionary<string, string>();
            foreach (string key in _request.Form.AllKeys)
            {
                parameters.Add(key, _request.Form.Get(key));
            }
            return parameters;
        }

        protected object BuildCookieObject(string name, string token)
        {
            return new HttpCookie(name, token);
        }

        protected void InsertCookie(string name, string token)
        {
            HttpCookie cookie = new HttpCookie(name, token);
            foreach (string key in _context.Request.Cookies.AllKeys)
            {
                if (key.Equals(name))
                {
                    _context.Response.Cookies.Remove(name);
                }
            }
            _context.Response.Cookies.Add(cookie);
        }

        protected string CookieValue(string name)
        {
            foreach (HttpCookie cookie in _context.Request.Cookies)
            {
                if (cookie.Name.Equals(name))
                {
                    return cookie.Value;
                }
            }
            return null;
        }

        protected void PrintCookies()
        {
            Console.WriteLine("COOKIES");
            foreach (var key in _request.Cookies.AllKeys)
            {
                Console.WriteLine("Key: {0} | Value: {1}", key, _request.Cookies.Get(key).Value);
            }
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

        public object Aux { get; set; }

        public Router Route { get; set; }

        public Dictionary<string, string> Session { get; set; }

        public Dictionary<string, string> User { get; set; }
    }
}

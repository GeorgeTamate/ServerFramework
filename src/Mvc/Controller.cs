using PHttp;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mvc
{
    /// <summary>
    /// Base controller class for applications.
    /// </summary>
    public class Controller
    {
        private HttpRequest _request;
        private HttpContext _context;


        /// <summary>
        /// Method that puts together the response view.
        /// </summary>
        /// <param name="layoutRoute">Path to the layout of the view.</param>
        /// <param name="viewRoute">Path to the partial view.</param>
        /// <param name="model">Object model with data to display in view.</param>
        /// <param name="viewData">Additional data to display in view.</param>
        /// <returns>Merged view as a string.</returns>
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

        /// <summary>
        /// Method that puts together a view with a message.
        /// </summary>
        /// <param name="layoutRoute">Path to the layout of the view.</param>
        /// <param name="partial">Path to the partial view.</param>
        /// <param name="message">Message to display in view.</param>
        /// <returns>Merged view as a string.</returns>
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

        /// <summary>
        /// Method that puts together a view for not found HTTP responses.
        /// </summary>
        /// <param name="msg">Message for the error.</param>
        /// <returns>Merged view as a string.</returns>
        public ActionResult NotFoundResult(string msg)
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";

            var result = new ActionResult(404, "Not Found");
            result.Content = MessageView(layoutPath, "<h1>404: Not Found</h1><br><p>{{Model.Msg}}</p>", msg);

            return result;
        }

        /// <summary>
        /// Method that puts together a view for unauthorized HTTP responses.
        /// </summary>
        /// <param name="msg">Message for the error.</param>
        /// <returns>Merged view as a string.</returns>
        public ActionResult UnauthorizedResult(string msg)
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";

            var result = new ActionResult(401, "Unauthorized");
            result.Content = MessageView(layoutPath, "<h1>401: Unauthorized</h1><br><p>{{Model.Msg}}</p>", msg);

            return result;
        }

        /// <summary>
        /// Parses parameters from the request body.
        /// </summary>
        /// <returns>Dictionary with parameters keys and values.</returns>
        protected Dictionary<string, string> PostParams()
        {
            var parameters = new Dictionary<string, string>();
            foreach (string key in _request.Form.AllKeys)
            {
                parameters.Add(key, _request.Form.Get(key));
            }
            return parameters;
        }

        /// <summary>
        /// Builds a HTTP cookie.
        /// </summary>
        /// <param name="name">Key of the cookie.</param>
        /// <param name="token">Value of the cookie.</param>
        /// <returns>Built cookie object</returns>
        protected object BuildCookieObject(string name, string token)
        {
            return new HttpCookie(name, token);
        }

        /// <summary>
        /// Inserts a cookie to the client context.
        /// </summary>
        /// <param name="name">Key of the cookie.</param>
        /// <param name="token">Value of the cookie.</param>
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

        /// <summary>
        /// Parses the value of a cookie.
        /// </summary>
        /// <param name="name">Key of the cookie to find.</param>
        /// <returns>Value of the cookie.</returns>
        protected string CookieValue(string name)
        {
            foreach (var key in _request.Cookies.AllKeys)
            {
                if (key.Equals(name))
                {
                    return _request.Cookies.Get(key).Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Prints request cookies in console.
        /// </summary>
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

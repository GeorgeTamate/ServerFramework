using Mvc;
using Mvc.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using JWT;
using JWT.Serializers;
using JWT.Algorithms;

namespace App1
{
    /// <summary>
    /// Class that represents the main controller of the application.
    /// </summary>
    public class HomeController : Controller
    {
        #region Basic EndPoints

        /// <summary>
        /// Default action method of the controller.
        /// </summary>
        /// <returns>Response as an action result.</returns>
        [HttpGET]
        public ActionResult Index()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/partial.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());

            return result;
        }

        /// <summary>
        /// Action method that provides information about the application.
        /// </summary>        
        /// <returns>Response as an action result.</returns>
        [HttpGET]
        public ActionResult About()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/about.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());

            PrintCookies();//TODO

            return result;

        }

        /// <summary>
        /// Action method that provides information about the application in JSON format.
        /// </summary>        
        /// <returns>Response as an action result.</returns>
        [HttpGET]
        public ActionResult Json()
        {
            var result = new JsonResult();
            var json = new JObject();

            json["name"] = "App1";
            json["author"] = "George Garcia Tamate";
            json["version"] = 0.7;
            result.Content = json.ToString();

            return result;
        }

        /// <summary>
        /// Action method that responds with a greeting message in text format.
        /// </summary>        
        /// <returns>Response as an action result.</returns>
        [HttpGET]
        public ActionResult Text()
        {
            var result = new ContentResult();
            result.Content = "Hello App1 !";

            return result;
        }

        #endregion
    }
}

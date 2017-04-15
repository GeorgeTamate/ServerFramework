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
    public class HomeController : Controller
    {
        [HttpGET]
        public ActionResult Index()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/partial.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());

            return result;
        }

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

        [HttpGET]
        public ActionResult Login()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/login.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());

            return result;
        }

        [HttpPOST]
        public ActionResult LoginPost()
        {
            var result = new JsonResult();
            var json = new JObject();
            var parameters = PostParams();
            foreach(var param in parameters)
            {
                json[param.Key] = param.Value;
            }

            string username = json["username"].ToString();
            string password = json["password"].ToString();

            var payload = new Dictionary<string, object>
            {
                { "username", username },
                { "password", password }
            };
            string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
            string name = "ShortApp1";

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            Console.WriteLine("TOKEN: {0}", token);


            //TODO


            result.Cookie = BuildCookieObject(name, token);
            result.Content = json.ToString();

            return result;
        }

        [HttpGET]
        public ActionResult Register()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/register.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());

            return result;
        }

        [HttpPOST]
        public ActionResult RegisterPost()
        {
            var result = new JsonResult();
            var json = new JObject();
            var parameters = PostParams();
            foreach (var param in parameters)
            {
                json[param.Key] = param.Value;
            }
            result.Content = json.ToString();

            if(!json["password"].Equals(json["confirm"]))
            {
                return UnauthorizedResult("Passwords are not the same.");
            }

            return result;
        }

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

        [HttpGET]
        public ActionResult Text()
        {
            var result = new ContentResult();
            result.Content = "Hello App1 !";

            return result;
        }
    }
}

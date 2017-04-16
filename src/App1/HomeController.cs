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
        #region Basic EndPoints

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

        #endregion

        #region Login

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
            // Parsing parameters from body
            var result = new JsonResult();
            var json = new JObject();
            var parameters = PostParams();
            foreach (var param in parameters)
            {
                json[param.Key] = param.Value;
            }

            string username = json["username"].ToString();
            string password = json["password"].ToString();

            DBHelper db = Aux as DBHelper;

            // Checking if user exists and if password is correct
            dynamic user = db.GetUser(username);
            if (user == null)
            {
                return UnauthorizedResult("This user does not exist.");
            }
            else if (!password.Equals(user.password))
            {
                return UnauthorizedResult("Invalid Password.");
            }

            // Creating and Encodin JWT token
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

            // Building cookie with JWT token
            result.Cookie = BuildCookieObject(name, token);
            result.Content = json.ToString();

            return result;
        }

        #endregion

        #region Register

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
            // Parsing parameters from body
            var result = new JsonResult();
            var json = new JObject();
            var parameters = PostParams();
            foreach (var param in parameters)
            {
                json[param.Key] = param.Value;
            }

            // Checking if passwords are the same
            if (!json["password"].Equals(json["confirm"]))
            {
                return UnauthorizedResult("Passwords are not the same.");
            }

            string username = json["username"].ToString();
            string password = json["password"].ToString();
            
            DBHelper db = Aux as DBHelper;

            // Checking if user already exists
            if (db.GetUser(username) != null)
            {
                return UnauthorizedResult("This user already exists.");
            }

            // Creating and Encodin JWT token
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

            // Inserting new user into Database
            db.CreateUser(username, password, token.Split('.')[2]);

            // Building cookie with JWT token
            result.Cookie = BuildCookieObject(name, token);
            result.Content = json.ToString();

            return result;
        }

        #endregion
    }
}

using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Mvc;
using Mvc.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace App1
{
    /// <summary>
    /// Class that represents the controller for the main functionalities of the application.
    /// </summary>
    public class ShortController : Controller
    {
        #region Login

        /// <summary>
        /// Action method that respondes with a view for logging in.
        /// </summary>
        /// <returns>Response as an action result.</returns>
        [HttpGET]
        public ActionResult Login()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/applayout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/login.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());

            return result;
        }

        /// <summary>
        /// Action method that responds with the login results.
        /// </summary>
        /// <returns>Response as an action result.</returns>
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

        /// <summary>
        /// Action method that respondes with a view for creating a new user.
        /// </summary>
        /// <returns>Response as an action result.</returns>
        [HttpGET]
        public ActionResult Register()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/applayout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/register.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());

            return result;
        }

        /// <summary>
        /// Action method that responds with the user creation results.
        /// </summary>
        /// <returns>Response as an action result.</returns>
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

        #region Create

        /// <summary>
        /// Action method to create a shortened URL.
        /// </summary>
        /// <returns>Response as an action result.</returns>
        [HttpGET]
        public ActionResult Create()
        {
            string token = CookieValue("ShortApp1");
            dynamic user = null;
            string layoutPath;
            string partialPath;
            DBHelper db = Aux as DBHelper;

            if (token != null)
            {
                user = db.GetUserBySecret(token.Split('.')[2]);
            }

            if (user != null)
            {
                layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/applayout.html";
                partialPath = Directory.GetCurrentDirectory() + "/../../../../view/create.html";
            }
            else
            {
                layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/captchalayout.html";
                partialPath = Directory.GetCurrentDirectory() + "/../../../../view/anoncreate.html";
            }
            
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, user);

            return result;
        }

        /// <summary>
        /// Action method that responds with the shortened URL creation results.
        /// </summary>
        /// <returns>Response as an action result.</returns>
        [HttpPOST]
        public ActionResult CreatePost()
        {
            // Parsing parameters from body
            var result = new JsonResult();
            var json = new JObject();
            var parameters = PostParams();
            string token = CookieValue("ShortApp1");
            dynamic user = null;
            DBHelper db = Aux as DBHelper;

            if (token != null)
            {
                user = db.GetUserBySecret(token.Split('.')[2]);
            }

            foreach (var param in parameters)
            {
                json[param.Key] = param.Value;
            }

            string link = json["link"].ToString();
            string shortlink = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Split('=')[0];
            json["short"] = "http://127.0.0.1:8085/app/" + shortlink;
            //json["short"] = "http://45.55.77.201:8085/app/" + shortlink;

            if (user != null)
            {
                db.CreateLink(shortlink, link, user.username);
            }
            else
            {
                db.CreateLink(shortlink, link, "");
            }
            
            result.Content = json.ToString();

            return result;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Action method to delete a shortened URL.
        /// </summary>
        /// <returns>Response as an action result.</returns>
        [HttpGET]
        public ActionResult Delete()
        {
            string token = CookieValue("ShortApp1");
            dynamic user = null;
            string layoutPath;
            string partialPath;
            DBHelper db = Aux as DBHelper;

            if (token != null)
            {
                user = db.GetUserBySecret(token.Split('.')[2]);
            }

            if (user != null)
            {
                layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/applayout.html";
                partialPath = Directory.GetCurrentDirectory() + "/../../../../view/delete.html";
            }
            else
            {
                return UnauthorizedResult("You have to be logged in to delete a shortened URL.");
            }

            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());

            return result;
        }

        /// <summary>
        /// Action method that responds with the shortened URL deletion results.
        /// </summary>
        /// <returns>Response as an action result.</returns>
        [HttpPOST]
        public ActionResult DeletePost()
        {
            // Parsing parameters from body
            var result = new JsonResult();
            var json = new JObject();
            var parameters = PostParams();
            string token = CookieValue("ShortApp1");
            dynamic user = null;
            DBHelper db = Aux as DBHelper;

            if (token != null)
            {
                user = db.GetUserBySecret(token.Split('.')[2]);
            }

            foreach (var param in parameters)
            {
                json[param.Key] = param.Value;
            }

            string shortlink = json["shortlink"].ToString();

            if (user != null)
            {
                db.DeleteLink(shortlink, user.username);
            }

            result.Content = json.ToString();

            return result;
        }

        #endregion

        /// <summary>
        /// Action method that logs out the logged in user.
        /// </summary>
        /// <returns>Response as an action result.</returns>
        [HttpGET]
        public ActionResult Logout()
        {
            string layoutPath = Directory.GetCurrentDirectory() + "/../../../../view/layout.html";
            string partialPath = Directory.GetCurrentDirectory() + "/../../../../view/logout.html";
            var result = new ActionResult();
            result.Content = View(layoutPath, partialPath, new object());
            result.Cookie = BuildCookieObject("ShortApp1","NA.NA.NA");
            return result;
        }

        /// <summary>
        /// Action method that returns the original URL to redirect to based in the shortened URL.
        /// </summary>
        /// <param name="url">Shortened URL.</param>
        /// <param name="db">Database Helper instance to look for the original URL.</param>
        /// <returns>Response as an action result.</returns>
        public ActionResult Redirect(string url, DBHelper db)
        {
            var result = new ActionResult();

            string link = db.GetLink(url);
            result.Redirect = link;

            return result;
        }

    }


}

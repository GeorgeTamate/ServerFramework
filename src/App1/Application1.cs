﻿using System;
using Mvc;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace App1
{
    /// <summary>
    /// Class that is intanciated to use application.
    /// </summary>
    public class Application1 : Application
    {
        private DBHelper _db;

        /// <summary>
        /// Method to be called when initiating application.
        /// </summary>
        public override void Start()
        {
            Console.WriteLine("   + App1 | Starting...");
            string configPath = Directory.GetCurrentDirectory() + "/../../../../App1Config.json";
            JObject configJson = JsonConvert.DeserializeObject(File.ReadAllText(configPath)) as JObject;
            _db = new DBHelper(configJson["dbPath"].ToString());
            _db.CreateDatabase();
            Console.WriteLine("   + App1 | Started!");
        }

        /// <summary>
        /// Method to be called when sending requests to the application.
        /// </summary>
        /// <param name="request">Object representing the HTTP request.</param>
        /// <param name="context">Object representing the context of the client making the request.</param>
        /// <returns>Object that carries the response to the request.</returns>
        public override object ExecuteAction(object request, object context)
        {
            string path = ParsePath(request);

            Console.WriteLine("-- ## {0} Reflection ExecuteAction!", ToString());
            if (path == null || path.Equals(""))
                return new HomeController().Index();

            Console.WriteLine("   + Path: {0}", path);

            var redir = new ShortController().Redirect(path, _db);
            if (redir.Redirect != null)
                return redir;

            var router = new Router(path);
            var result = router.CallAction(GetType(), request, context, _db);

            return result;
        }
    }
}

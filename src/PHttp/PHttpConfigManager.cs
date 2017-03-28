﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PHttp
{
    public struct Site
    {
        public string Name;
        public string PhysicalPath;
        public string VirtualPath;
        public bool DirectoryBrowsing;
        public List<string> DefaultDocument;
        public Dictionary<int, string> ErrorPages;
    }

    public class PHttpConfigManager
    {
        private string _path;

        public PHttpConfigManager() : 
            this(Directory.GetCurrentDirectory() + "/../../../../config.json")//debug/bin/client/ServerFramework
        { }

        public PHttpConfigManager(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("** PHttpConfigManager Exception | Configuration file not found.");
                throw new FileNotFoundException("Configuration file not found.");
            }

            _path = configFilePath;
            DefaultDocument = new List<string>();
            ErrorPages = new Dictionary<int, string>();
            Sites = new List<Site>();
            Init();
        }

        private void Init()
        {
            // Getting general JSON string from file
            JObject configJson = JsonConvert.DeserializeObject(File.ReadAllText(_path)) as JObject;

            // Assigning port from JSON
            Port = int.Parse(configJson["port"].ToString());

            // Populating defaultDocument List
            foreach (var document in configJson["defaultDocument"] as JArray)
                DefaultDocument.Add(document.ToString());

            // Populating errorPages Dictionary
            JProperty property;
            foreach (var token in configJson["errorPages"] as JArray)
            {
                property = token.First.Value<JProperty>();
                ErrorPages.Add(int.Parse(property.Name), property.Value.ToString());
            }

            // Populatin sites Struct List
            Site site = new Site();
            foreach (var token in configJson["sites"] as JArray)
            {
                site.DefaultDocument = new List<string>();
                site.ErrorPages = new Dictionary<int, string>();
                site.Name = token["name"].ToString();
                site.PhysicalPath = token["physicalPath"].ToString();
                site.VirtualPath = token["virtualPath"].ToString();
                site.DirectoryBrowsing = bool.Parse(token["directoryBrowsing"].ToString());

                foreach (var document in token["defaultDocument"] as JArray)
                    site.DefaultDocument.Add(document.ToString());

                foreach (var pages in token["errorPages"] as JArray)
                {
                    property = pages.First.Value<JProperty>();
                    site.ErrorPages.Add(int.Parse(property.Name), property.Value.ToString());
                }
                
                Sites.Add(site);
            }

            //Printing Config
            Console.WriteLine("Port: {0}", Port);
            foreach (var document in DefaultDocument)
                Console.WriteLine("SiteDocument: {0}", document);
            foreach (var pair in ErrorPages)
                Console.WriteLine("ErrorPage: {0}", pair);
            foreach (var s in Sites)
            {
                Console.WriteLine("Site: {0}, physical:{1}, virtual:{2}, browsing:{3}", s.Name, s.PhysicalPath, s.VirtualPath, s.DirectoryBrowsing);
                foreach (var document in s.DefaultDocument)
                    Console.WriteLine("SiteDocument: {0}", document);
                foreach (var pages in s.ErrorPages)
                    Console.WriteLine("SiteErrorPage: {0}", pages);
            }
        }

        #region Properties

        public int Port { get; private set; }

        public List<string> DefaultDocument { get; private set; }

        public Dictionary<int, string> ErrorPages { get; private set; }

        public List<Site> Sites { get; private set; }

        #endregion

    }
}

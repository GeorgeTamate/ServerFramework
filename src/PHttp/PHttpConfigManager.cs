using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

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

    //public class PartialConfig
    //{
    //    public List<string> DefaultDocument { get; set; }
    //    public int Port { get; set; }
    //}

    public class PHttpConfigManager
    {
        private Dictionary<int, string> _errorPages = new Dictionary<int, string>();
        private List<Site> _sites = new List<Site>();
        private int _port;
        private List<string> _defaultDocument = new List<string>();
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
            Init();
        }

        private void Init()
        {
            //PartialConfig config = JsonConvert.DeserializeObject<PartialConfig>(File.ReadAllText(_path));
            //_port = config.Port;
            //_defaultDocument = config.DefaultDocument;


            // Getting general JSON string from file
            JObject configJson = JsonConvert.DeserializeObject(File.ReadAllText(_path)) as JObject;

            // Assigning port from JSON
            _port = int.Parse(configJson["port"].ToString());

            // Populating defaultDocument List
            foreach (var document in configJson["defaultDocument"] as JArray)
                _defaultDocument.Add(document.ToString());

            // Populating errorPages Dictionary
            foreach (var token in configJson["errorPages"] as JArray)
                _errorPages.Add(int.Parse(Regex.Match(token.First.ToString().Split(':')[0], @"\d+").Value), token.First.First.ToString());

            // Populatin sites Struct List
            Site site = new Site();
            JArray sitesJson = configJson["sites"] as JArray;
            foreach (var token in sitesJson)
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
                    site.ErrorPages.Add(int.Parse(Regex.Match(pages.First.ToString().Split(':')[0], @"\d+").Value), pages.First.First.ToString());

                _sites.Add(site);
            }

            //Printing Config
            Console.WriteLine("Port: {0}", _port);
            foreach (var document in _defaultDocument)
                Console.WriteLine("SiteDocument: {0}", document);
            foreach (var pair in _errorPages)
                Console.WriteLine("ErrorPage: {0}", pair);
            foreach (var s in _sites)
            {
                Console.WriteLine("Site: {0}, {1}, {2}, {3}", s.Name, s.PhysicalPath, s.VirtualPath, s.DirectoryBrowsing);
                foreach (var document in s.DefaultDocument)
                    Console.WriteLine("SiteDocument: {0}", document);
                foreach (var pages in s.ErrorPages)
                    Console.WriteLine("SiteErrorPage: {0}", pages);
            }


            
        }

    }
}

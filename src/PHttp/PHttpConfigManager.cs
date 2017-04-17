using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PHttp
{
    /// <summary>
    /// Struct that represents the configuration to be loaded for an application.
    /// </summary>
    public struct Site
    {
        public string Name;
        public string PhysicalPath;
        public string VirtualPath;
        public bool DirectoryBrowsing;
        public List<string> DefaultDocument;
        public Dictionary<int, string> ErrorPages;
    }

    /// <summary>
    /// Class to load the configuration for the server and the applications it hosts.
    /// </summary>
    public class PHttpConfigManager
    {
        private string _path;

        /// <summary>
        /// Constructor with default path to configuration file.
        /// </summary>
        public PHttpConfigManager() : 
            this(Directory.GetCurrentDirectory() + "/../../../../config.json")//debug/bin/client/ServerFramework
        { }


        /// <summary>
        /// Constructor with custom path to confuguration file.
        /// </summary>
        /// <param name="configFilePath">Path to the configuration file.</param>
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

        /// <summary>
        /// Initiates the server configuration and applications configuration.
        /// </summary>
        private void Init()
        {
            Console.WriteLine("+-+-+ INITIATING CONFIGURATION MANAGER +-+-+");
            Console.WriteLine("-- Loading configuration from JSON configuration file...");
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
            Console.WriteLine("  + Port: {0}", Port);
            foreach (var document in DefaultDocument)
                Console.WriteLine("  + Default Document: {0}", document);
            foreach (var page in ErrorPages)
                Console.WriteLine("  + Error Page: {0}", page);
            int i = 0;
            foreach (var s in Sites)
            {
                Console.WriteLine("  + Site [{0}] | Name: {1}", i, s.Name);
                Console.WriteLine("  + Site [{0}] | Physical Path: {1}", i, s.PhysicalPath);
                Console.WriteLine("  + Site [{0}] | Virtual Path: {1}", i, s.VirtualPath);
                Console.WriteLine("  + Site [{0}] | Directory Browsing: {1}", i, s.DirectoryBrowsing);
                foreach (var document in s.DefaultDocument)
                    Console.WriteLine("  + SiteDocument: {0}", document);
                foreach (var pages in s.ErrorPages)
                    Console.WriteLine("  + SiteErrorPage: {0}", pages);
                i++;
            }
            Console.WriteLine("-- Configuration Loading COMPLETE!");
            Console.WriteLine("-");
        }

        /// <summary>
        /// Finds the text content of default document files.
        /// </summary>
        /// <param name="path">path to the document.</param>
        /// <returns>Text content of the document.</returns>
        public string FindDefaultDocumentText(string path)
        {
            foreach (var document in DefaultDocument)
            {
                if (File.Exists(path + "/" + document))
                {
                    return File.ReadAllText(path + "/" + document);
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the name of default document files.
        /// </summary>
        /// <param name="path">path to the document.</param>
        /// <returns>Name of the document.</returns>
        public string FindDefaultDocumentFileName(string path)
        {
            foreach (var document in DefaultDocument)
            {
                if (File.Exists(path + "/" + document))
                {
                    return document;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the text content of error pages files.
        /// </summary>
        /// <param name="path">path to the error pages.</param>
        /// <returns>Text content of the error pages.</returns>
        public string FindErrorPageText(int statusCode, string path)
        {
            foreach (var page in ErrorPages)
            {
                if (page.Key.Equals(statusCode))
                {
                    return File.ReadAllText(path + "/" + page.Value);
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the name of error pages files.
        /// </summary>
        /// <param name="path">path to the error page.</param>
        /// <returns>Name of the error page.</returns>
        public string FindErrorPageFileName(int statusCode)
        {
            string fileName = null;
            if (ErrorPages.ContainsKey(statusCode))
            {
                ErrorPages.TryGetValue(statusCode, out fileName);
            }
            return fileName;
        }

        #region Properties

        public int Port { get; private set; }

        public List<string> DefaultDocument { get; private set; }

        public Dictionary<int, string> ErrorPages { get; private set; }

        public List<Site> Sites { get; private set; }

        #endregion

    }
}

using PHttp.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PHttp
{
    /// <summary>
    /// Class that initializes the applications to be hosted by the server.
    /// </summary>
    public class Startup
    {
        private Dictionary<string, IPHttpApplication> _instances = new Dictionary<string, IPHttpApplication>();

        /// <summary>
        /// Method that loads and initializes the applications.
        /// </summary>
        /// <param name="config">Configuration of server and applications.</param>
        public void LoadApps(PHttpConfigManager config)
        {
            DirectoryInfo info;
            _instances = new Dictionary<string, IPHttpApplication>(); //list of applications compatible with IPHttpApplication

            Console.WriteLine("+-+-+ INITIATE SITE APPLICATIONS +-+-+");
            Console.WriteLine("-- Loading applications in Configuration Manager through Reflection...");

            foreach (var site in config.Sites)
            {
                if (string.IsNullOrEmpty(site.PhysicalPath))
                {
                    Console.WriteLine("** The physical app path field for '{0}' is null or empty.", site.Name);
                    return;
                } //sanity check: is a physical path defined for this site.

                info = new DirectoryInfo(site.PhysicalPath);

                if (!info.Exists)
                {
                    Console.WriteLine("** Directory in '{0}' physical path does not exist.", site.Name);
                    return;
                } //make sure directory exists
                Console.WriteLine("-- Directory for '{0}' found.", site.Name);
                Console.WriteLine("-- Loading assembly files (*.dll) for '{0}'...", site.Name);

                foreach (FileInfo file in info.GetFiles(site.Name + ".dll")) //loop through all dll files in directory
                {
                    Assembly currentAssembly = null;
                    try
                    {
                        //Console.WriteLine(file.FullName);
                        var name = AssemblyName.GetAssemblyName(file.FullName);
                        currentAssembly = Assembly.Load(name);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("** Failed to load Assembly for FileInfo: {0}", file.FullName);
                        continue;
                    }

                    Console.WriteLine("-- App: '{0}', Assembly: '{1}' | Assembly loaded successfully.", site.Name, file.Name);
                    Console.WriteLine("-- Looking for types compatible with IPHttpApplication...", site.Name);

                    var types = currentAssembly.GetTypes();
                    foreach (Type t in types)
                    {
                        if (t != typeof(IPHttpApplication) && typeof(IPHttpApplication).IsAssignableFrom(t))
                        {
                            _instances.Add(site.VirtualPath, (IPHttpApplication)Activator.CreateInstance(t));
                            Console.WriteLine("-- + Found Type '{0}' and added it to the App List.", t.ToString());
                        }
                    }
                }
            }

            Console.WriteLine("-- Applications Loading COMPLETE!");
            Console.WriteLine("   Listing loaded applications:");

            foreach (var ins in _instances)
            {
                Console.WriteLine("   + Instance Type: {0} | App Virtual Path: {1}", ins.Value.ToString(), ins.Key);
            }

            Console.WriteLine("-- Starting Applications...");
            foreach (var ins in _instances)
            {
                ins.Value.Start();
            }

            Console.WriteLine("-- Done!");
            Console.WriteLine("-");
        }

        /// <summary>
        /// Method that invokes the ExecuteAction method of loaded applications.
        /// </summary>
        /// <param name="request">Request to send to application.</param>
        /// <param name="context">Context of the client making the request.</param>
        /// <returns>Response from the application.</returns>
        public object InvokeApp(HttpRequest request, HttpContext context)
        {
            object result = null;
            string virtualPath = request.Path.Split('/')[1];
            foreach (var ins in _instances)
            {
                if (ins.Key.Equals(virtualPath))
                {
                    Console.WriteLine("-- Invoking App Method | Request Virtual Path: '{0}', Matching Instance: {1}", request.Path.Split('/')[1], ins.ToString());
                    result = ins.Value.ExecuteAction(request, context); //request.Path.Remove(0, virtualPath.Length + 2)
                    break;
                }
            }
            return result;
        }
    }
}

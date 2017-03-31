using PHttp.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PHttp
{
    public class Startup
    {
        private string _path = null;
        private List<IPHttpApplication> _instances = new List<IPHttpApplication>();

        public Startup(string path)
        {
            _path = path;
        }

        public void LoadApps1()
        {
            if (string.IsNullOrEmpty(_path))
            {
                Console.WriteLine("** Startup._path field is null or empty.");
                return;
            } //sanity check
            Console.WriteLine("-- Path entry recieved.");

            DirectoryInfo info = new DirectoryInfo(_path);

            if (!info.Exists)
            {
                Console.WriteLine("** Path in Startup._path does not exist.");
                return;
            } //make sure directory exists
            Console.WriteLine("-- Path in Startup._path found.");

            var instanceList = new List<IPHttpApplication>(); //list of applications compatible with IPHttpApplication

            foreach (FileInfo file in info.GetFiles("*.dll")) //loop through all dll files in directory
            {
                Assembly currentAssembly = null;
                try
                {
                    var name = AssemblyName.GetAssemblyName(file.FullName);
                    currentAssembly = Assembly.Load(name);
                }
                catch (Exception)
                {
                    Console.WriteLine("** Failed to load Assembly for FileInfo: {0}", file.FullName);
                    continue;
                }

                var types = currentAssembly.GetTypes();
                foreach(Type t in types)
                {
                    if(t != typeof(IPHttpApplication) && typeof(IPHttpApplication).IsAssignableFrom(t))
                    {
                        instanceList.Add((IPHttpApplication)Activator.CreateInstance(t));
                    }
                }

                //currentAssembly.GetTypes()
                //    .Where(t => t != typeof(IPHttpApplication) && typeof(IPHttpApplication).IsAssignableFrom(t)) //type cannot be IPHttpApplication, but has to be compatible with it.
                //    .ToList()
                //    .ForEach(x => instanceList.Add((IPHttpApplication)Activator.CreateInstance(x))); //instanciate and add to list of types.
            }

            Console.WriteLine("-- Apps Loading Complete!");
            Console.WriteLine("   Calling Start() method for every type instance in list:");

            foreach (var ins in instanceList)
            {
                Console.Write("   + Name: {0} | Start(): ", ins.Name);
                ins.Start();
            }

            Console.WriteLine("-- Done!");
        }





        public void LoadApps(PHttpConfigManager config)
        {
            DirectoryInfo info;
            _instances = new List<IPHttpApplication>(); //list of applications compatible with IPHttpApplication

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

                foreach (FileInfo file in info.GetFiles("*.dll")) //loop through all dll files in directory
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
                            _instances.Add((IPHttpApplication)Activator.CreateInstance(t));
                            Console.WriteLine("-- + Found Type '{0}' and added it to the App List.", t.ToString());
                        }
                    }
                }
            }

            Console.WriteLine("-- Applications Loading COMPLETE!");
            Console.WriteLine("   Listing loaded applications:");

            foreach (var ins in _instances)
            {
                Console.WriteLine("   + Instance Type: {0} | App Virtual Path: {1}", ins.ToString(), ins.Name);
            }

            Console.WriteLine("-- Done!");
            Console.WriteLine("-");

            //string response = null;
            //foreach (var ins in _instances)
            //{
            //    //Console.WriteLine("---- InsName: {0}, RequestVirtual: {1} ----", ins.Name, request.Split('/')[1]);
            //    if (ins.Name.Equals(request.Split('/')[1]))
            //    {
            //        response = ins.ExecuteAction();
            //        break;
            //    }
            //}

            //return response;
        }

        public string InvokeApp(string request)
        {
            string response = null;
            foreach (var ins in _instances)
            {
                if (ins.Name.Equals(request.Split('/')[1]))
                {
                    Console.WriteLine("-- Invoking App Method | Request Virtual Path: '{0}', Matching Instance: {1}", request.Split('/')[1], ins.ToString());
                    response = ins.ExecuteAction();
                    break;
                }
            }

            return response;
        }
    }
}

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

        public Startup(string path)
        {
            _path = path;
        }

        public void LoadApps()
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





        public string CallApp(PHttpConfigManager config, string request)
        {
            DirectoryInfo info;
            var instanceList = new List<IPHttpApplication>(); //list of applications compatible with IPHttpApplication

            foreach (var site in config.Sites)
            {
                if (string.IsNullOrEmpty(site.PhysicalPath))
                {
                    Console.WriteLine("** The physical app path field for '{0}' is null or empty.", site.Name);
                    return null;
                } //sanity check
                Console.WriteLine("-- '{0}' path entry recieved.", site.Name);

                info = new DirectoryInfo(site.PhysicalPath);

                if (!info.Exists)
                {
                    Console.WriteLine("** Path in '{0}' physical path does not exist.", site.Name);
                    return null;
                } //make sure directory exists
                Console.WriteLine("-- Path in '{0}' found.", site.Name);

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

                    var types = currentAssembly.GetTypes();
                    foreach (Type t in types)
                    {
                        if (t != typeof(IPHttpApplication) && typeof(IPHttpApplication).IsAssignableFrom(t))
                        {
                            instanceList.Add((IPHttpApplication)Activator.CreateInstance(t));
                        }
                    }
                }



            }

            

            Console.WriteLine("-- Apps Loading Complete!");
            Console.WriteLine("   Calling ExecuteAction() method for specified type instance in list:");

            foreach (var ins in instanceList)
            {
                Console.WriteLine("   + Virtual Path: {0} | Instance: {1}", ins.Name, ins.ToString());
            }

            Console.WriteLine("-- Done!");

            string response = null;
            foreach (var ins in instanceList)
            {
                //Console.WriteLine("---- InsName: {0}, RequestVirtual: {1} ----", ins.Name, request.Split('/')[1]);
                if (ins.Name.Equals(request.Split('/')[1]))
                {
                    response = ins.ExecuteAction();
                    break;
                }
            }

            return response;
        }
    }
}

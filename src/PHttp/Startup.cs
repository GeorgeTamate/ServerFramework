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

    }
}

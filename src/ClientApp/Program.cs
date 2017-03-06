using System;
using System.Configuration;
using PHttp;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = ConfigurationManager.AppSettings["ApplicationsDir"].ToString();
            Console.WriteLine("Path directory: {0}", path);

            var startup = new Startup(path);
            startup.LoadApps();

            //Console.WriteLine("Press any key to finish...");
            //Console.ReadKey();
        }
    }
}
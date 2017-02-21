using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PHttp;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["ApplicationsDir"].ToString();
            Console.WriteLine($"Path directory: {path}");

            var startup = new Startup(path);
            startup.LoadApps();

            Console.WriteLine($"Press any key to finish...");
            Console.ReadKey();
        }
    }
}

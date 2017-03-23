using System;
using System.Configuration;
using PHttp;
using System.IO;
using System.Diagnostics;

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

            Console.WriteLine("Press any key to start server...");//
            Console.ReadKey();//

            //////////////////////////////////////////////////////////////////

            int port = 8085;

            using (var server = new HttpServer(port)) //TODO 
            {
                // New requests are signaled through the RequestReceived
                // event.

                server.RequestReceived += (s, e) =>
                {
                    // The response must be written to e.Response.OutputStream.
                    // When writing text, a StreamWriter can be used.

                    using (var writer = new StreamWriter(e.Response.OutputStream))
                    {
                        writer.Write("Hello world!");
                    }
                };

                server.StateChanged += (sender, e) =>
                {
                    var st = e as StateChangedEventArgs;

                    Console.Write("-- State Changed | ");
                    Console.Write(st.PreviousState.ToString());
                    Console.Write(" -> ");
                    Console.Write(st.CurrentState.ToString());
                    Console.WriteLine();
                };

                // Start the server on a random port. Use server.EndPoint
                // to specify a specific port, e.g.:
                //
                //     server.EndPoint = new IPEndPoint(IPAddress.Loopback, 80);
                //

                server.Start();

                // Start the default web browser.

                Process.Start(String.Format("http://{0}/", server.EndPoint));

                Console.WriteLine("Press any key to stop server...");
                Console.ReadKey();

                // When the HttpServer is disposed, all opened connections
                // are automatically closed.

                server.Stop();

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
using System;
using PHttp.Application;
using PHttp;

namespace Mvc
{
    /// <summary>
    /// Application base class.
    /// </summary>
    public class Application : IPHttpApplication
    {
        string virtualPath = "";

        /// <summary>
        /// Method to be called when initiating application.
        /// </summary>
        public virtual void Start()
        {
            Console.WriteLine("## Reflection Start!");
        }

        /// <summary>
        /// Method to be called when sending requests to the application.
        /// </summary>
        /// <param name="request">Object representing the HTTP request.</param>
        /// <param name="context">Object representing the context of the client making the request.</param>
        /// <returns>Object that carries the response to the request.</returns>
        public virtual object ExecuteAction(object request, object context)
        {
            Console.WriteLine("## Reflection ExecuteAction!");
            return "success";
        }

        public event ApplicationStartMethod applicationStartMethod;
        public event PreApplicationStartMethod preApplicationStartMethod;

        protected string ParsePath(object request)
        {
            try
            {
                string virtualPath = ((HttpRequest)request).Path.Split('/')[1];
                return ((HttpRequest)request).Path.Remove(0, virtualPath.Length + 2);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Name
        {
            get { return virtualPath; }
            set { virtualPath = value; }
        }
    }
}

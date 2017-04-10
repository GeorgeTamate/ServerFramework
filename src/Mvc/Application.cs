using System;
using PHttp.Application;
using PHttp;

namespace Mvc
{
    public class Application : IPHttpApplication
    {
        string virtualPath = "";

        public virtual void Start()
        {
            Console.WriteLine("## Reflection Start!");
        }

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

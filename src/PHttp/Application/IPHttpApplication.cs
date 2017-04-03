using System;

namespace PHttp.Application
{
    public interface IPHttpApplication
    {
        void Start();

        object ExecuteAction(object request, object context);

        string Name { get; set; }

        event PreApplicationStartMethod preApplicationStartMethod;

        event ApplicationStartMethod applicationStartMethod;
    }

    public delegate void PreApplicationStartMethod(Type type, string method);
    public delegate void ApplicationStartMethod(Type type, string method);


}

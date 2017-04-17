using System;

namespace PHttp.Application
{
    /// <summary>
    /// Interface for objects classes that will represent the applications' endpoint of call from the server.
    /// </summary>
    public interface IPHttpApplication
    {
        /// <summary>
        /// Method to be called when initiating application.
        /// </summary>
        void Start();

        /// <summary>
        /// Method to be called when sending requests to the application.
        /// </summary>
        /// <param name="request">Object representing the HTTP request.</param>
        /// <param name="context">Object representing the context of the client making the request.</param>
        /// <returns>Object that carries the response to the request.</returns>
        object ExecuteAction(object request, object context);

        string Name { get; set; }

        event PreApplicationStartMethod preApplicationStartMethod;

        event ApplicationStartMethod applicationStartMethod;
    }

    public delegate void PreApplicationStartMethod(Type type, string method);
    public delegate void ApplicationStartMethod(Type type, string method);


}

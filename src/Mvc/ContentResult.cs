
namespace Mvc
{
    /// <summary>
    /// Class that represents text responses made by the application.
    /// </summary>
    public class ContentResult : ActionResult
    {
        /// <summary>
        /// Constructor of the class with default content type.
        /// </summary>
        public ContentResult() : base("text/plain")
        {
        }

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="statusCode">Custom status code.</param>
        /// <param name="statusDescription">Custom status description.</param>
        public ContentResult(int statusCode, string statusDescription) : this()
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }
    }
}

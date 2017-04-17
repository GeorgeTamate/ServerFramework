
namespace Mvc
{
    /// <summary>
    /// Class that represents JSON responses made by the application.
    /// </summary>
    public class JsonResult : ActionResult
    {
        /// <summary>
        /// Constructor of the class with default content type.
        /// </summary>
        public JsonResult() : base("application/json")
        {
        }

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="statusCode">Custom status code.</param>
        /// <param name="statusDescription">Custom status description.</param>
        public JsonResult(int statusCode, string statusDescription) : this()
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }
    }
}


namespace Mvc
{
    /// <summary>
    /// Class that represents the response made by the application.
    /// </summary>
    public class ActionResult
    {
        /// <summary>
        /// Constructor that initializes properties of the class.
        /// </summary>
        public ActionResult()
        {
            StatusCode = 200;
            StatusDescription = "OK";
            ContentType = "text/html";
            Content = null;
            Cookie = null;
            Redirect = null;
        }

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="contentType">Custom content type.</param>
        protected ActionResult(string contentType) : this()
        {
            ContentType = contentType;
        }

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="statusCode">Custom status code.</param>
        /// <param name="statusDescription">Custom status description.</param>
        public ActionResult(int statusCode, string statusDescription) : this()
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }

        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public string ContentType { get; set; }

        public string Content { get; set; }

        public object Cookie { get; set; }

        public string Redirect { get; set; }
    }
}


namespace Mvc
{
    public class ContentResult : ActionResult
    {
        public ContentResult() : base("text/plain")
        {
        }

        public ContentResult(int statusCode, string statusDescription) : this()
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }
    }
}

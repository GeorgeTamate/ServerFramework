
namespace Mvc
{
    public class JsonResult : ActionResult
    {
        public JsonResult() : base("application/json")
        {
        }

        public JsonResult(int statusCode, string statusDescription) : this()
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }
    }
}

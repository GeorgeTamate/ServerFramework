
namespace Mvc.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class HttpDELETE : System.Attribute
    {
        private string verb;

        public HttpDELETE()
        {
            verb = "DELETE";
        }
    }
}


namespace Mvc.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class HttpGET : System.Attribute
    {
        private string verb;

        public HttpGET()
        {
            verb = "GET";
        }
    }
}

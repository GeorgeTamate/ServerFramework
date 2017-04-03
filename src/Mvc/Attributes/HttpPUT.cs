
namespace Mvc.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class HttpPUT : System.Attribute
    {
        private string verb;

        public HttpPUT()
        {
            verb = "PUT";
        }
    }
}

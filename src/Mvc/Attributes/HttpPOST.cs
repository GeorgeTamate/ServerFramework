
namespace Mvc.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class HttpPOST: System.Attribute
    {
        private string verb;

        public HttpPOST()
        {
            verb = "POST";
        }
    }
}

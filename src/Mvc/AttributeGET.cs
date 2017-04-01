
namespace Mvc
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class AttributeGET : System.Attribute
    {
        private string verb;

        public AttributeGET()
        {
            verb = "GET";
        }
    }
}

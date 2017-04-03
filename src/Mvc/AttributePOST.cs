
namespace Mvc
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class AttributePOST : System.Attribute
    {
        private string verb;

        public AttributePOST()
        {
            verb = "POST";
        }
    }
}


namespace Mvc
{
    public class ActionResult
    {
        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public string ContentType { get; set; }
         
        public string Content { get; set; }
    }
}

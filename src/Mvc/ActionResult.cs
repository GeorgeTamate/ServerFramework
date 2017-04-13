﻿
namespace Mvc
{
    public class ActionResult
    {
        public ActionResult()
        {
            StatusCode = 200;
            StatusDescription = "OK";
            ContentType = "text/html";
            Content = null;
            Cookie = null;
        }

        protected ActionResult(string contentType) : this()
        {
            ContentType = contentType;
        }

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
    }
}

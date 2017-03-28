namespace PHttp
{
    public class AppResponse
    {
        public AppResponse()
        {

        }

        public AppResponse(string content, string contentType, int statusCode, string statusDescription)
        {
            Content = content;
            ContentType = contentType;
            StatusCode = statusCode;
            StatusDescription = StatusDescription;
        }

        public AppResponse GetResponse(string request)
        {
            return new AppResponse(null, null, 0, null);
        }


        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public string ContentType { get; set; }

        public string Content { get; set; }
    }
}

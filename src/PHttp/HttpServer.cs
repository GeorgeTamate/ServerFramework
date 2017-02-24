using System;
using System.Net;

namespace PHttp
{
    public class HttpServer : IDisposable
    {
        public HttpServer()
        {
            EndPoint = new IPEndPoint(IPAddress.Loopback, 0);
            ReadBufferSize = 4096;
            WriteBufferSize = 4096;
            ShutdownTimeout = TimeSpan.FromSeconds(30);
            ReadTimeout = TimeSpan.FromSeconds(90);
            WriteTimeout = TimeSpan.FromSeconds(90);
            ServerBanner = String.Format("PHttp/{0}", GetType().Assembly.GetName().Version);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void VerifyState(HttpServerState state)
        {
            throw new NotImplementedException();
        }

        private void StopClients()
        {
            throw new NotImplementedException();
        }

        private void BeginAcceptTcpClient()
        {
            throw new NotImplementedException();
        }

        private void AcceptTcpClientCallback(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        private void RegisterClient(HttpClient client)
        {
            throw new NotImplementedException();
        }

        internal void UnregisterClient(HttpClient client)
        {
            throw new NotImplementedException();
        }


        public IPEndPoint EndPoint { get; set; }

        public int ReadBufferSize { get; set; }

        public int WriteBufferSize { get; set; }

        public string ServerBanner { get; set; }

        public TimeSpan ReadTimeout { get; set; }

        public TimeSpan WriteTimeout { get; set; }

        public TimeSpan ShutdownTimeout { get; set; }

        internal HttpServerUtility ServerUtility { get; set; }

        internal HttpTimeoutManager TimeoutManager { get; set; }
    }
}

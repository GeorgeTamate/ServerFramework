using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PHttp
{
    public class HttpServer : IDisposable
    {
        #region Members

        private TcpListener _listener;
        private AutoResetEvent _clientsChangedEvent = new AutoResetEvent(false);
        private bool _disposed = false;
        private HttpServerState _state = HttpServerState.Stopped;

        #endregion

        #region Constructors

        public HttpServer()
            : this(0)
        {
        }

        public HttpServer(int port)
        {
            Port = port;
            State = HttpServerState.Stopped;
            EndPoint = new IPEndPoint(IPAddress.Loopback, Port);
            ReadBufferSize = 4096;
            WriteBufferSize = 4096;
            ShutdownTimeout = TimeSpan.FromSeconds(30);
            ReadTimeout = TimeSpan.FromSeconds(90);
            WriteTimeout = TimeSpan.FromSeconds(90);
            ServerBanner = String.Format("PHttp/{0}", GetType().Assembly.GetName().Version);
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            if (!_disposed)
            {
                if (State == HttpServerState.Started)
                    Stop();
                if (_clientsChangedEvent != null)
                {
                    ((IDisposable)_clientsChangedEvent).Dispose();
                    _clientsChangedEvent = null;
                }
            }

            if (TimeoutManager != null)
            {
                TimeoutManager.Dispose();
                TimeoutManager = null;
            }

            _disposed = true;
        }

        public void Start()
        {
            VerifyState(HttpServerState.Stopped);
            State = HttpServerState.Starting;
            Console.WriteLine("-- Server Starting at EndPoint {0}:{1}", EndPoint.Address, EndPoint.Port);
            TimeoutManager = new HttpTimeoutManager(this);
            var listener = new TcpListener(EndPoint);

            try
            {
                listener.Start();
                EndPoint = listener.LocalEndpoint as IPEndPoint;
                _listener = listener;
                ServerUtility = new HttpServerUtility();
                Console.WriteLine("-- Server Running at EndPoint {0}:{1}", EndPoint.Address, EndPoint.Port);
                State = HttpServerState.Started;
                BeginAcceptTcpClient();
            }
            catch (Exception e)
            {
                State = HttpServerState.Stopped;
                Console.WriteLine("** The Server failed to start. | Exception: " + e.Message);
                throw new PHttpException("The Server failed to start.");
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void VerifyState(HttpServerState state)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
            if (_state != state)
                throw new InvalidOperationException(String.Format("Expected server to be in the '{0}' state", state));
        }

        private void StopClients()
        {
            throw new NotImplementedException();
        }

        private void BeginAcceptTcpClient()
        {
            var listener = _listener;
            if (listener != null)
            {
                listener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
            }
        }

        private void AcceptTcpClientCallback(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        private void RegisterClient(HttpClient client)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internal Methods

        internal void UnregisterClient(HttpClient client)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Events and Handlers

        public event EventHandler StateChanged;
        protected virtual void OnChangedState(EventArgs args)
        {
            var ev = StateChanged;
            if (ev != null)
            {
                ev(this, args);
            }
        }

        #endregion

        #region Properties

        public HttpServerState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnChangedState(EventArgs.Empty);
                }
            }
        }

        public int Port { get; private set; }

        public IPEndPoint EndPoint { get; private set; }

        public int ReadBufferSize { get; set; }

        public int WriteBufferSize { get; set; }

        public string ServerBanner { get; set; }

        public TimeSpan ReadTimeout { get; set; }

        public TimeSpan WriteTimeout { get; set; }

        public TimeSpan ShutdownTimeout { get; set; }

        internal HttpServerUtility ServerUtility { get; private set; }

        internal HttpTimeoutManager TimeoutManager { get; private set; }

        #endregion
    }
}

using System;

namespace PHttp
{
    public class StateChangedEventArgs : EventArgs
    {
        // Constructor

        public StateChangedEventArgs(HttpServerState previousState, HttpServerState currentState)
        {
            PreviousState = previousState;
            CurrentState = currentState;
        }

        // Properties

        public HttpServerState CurrentState { get; private set; }

        public HttpServerState PreviousState { get; private set; }
    }
}

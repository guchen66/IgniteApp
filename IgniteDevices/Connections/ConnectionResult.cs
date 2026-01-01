namespace IgniteDevices.Connections
{
    public class ConnectionResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public ConnectionResult(bool isSucess, string message)
        {
            IsSuccess = isSucess;
            Message = message;
        }
    }
}
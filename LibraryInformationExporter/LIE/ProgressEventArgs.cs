namespace LIE
{
    public class ProgressEventArgs
    {
        public string Message { get; }

        public ProgressEventArgs(string message)
        {
            Message = message;
        }
    }
}
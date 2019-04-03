namespace LIE
{
    public class TagProviderProgressEventArgs
    {
        public string Message { get; }

        public TagProviderProgressEventArgs(string message)
        {
            Message = message;
        }
    }
}
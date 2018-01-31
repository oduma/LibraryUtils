namespace Sciendo.Common.Music.Contracts
{
    public interface IPostProcessor
    {
        bool Process(object messageContents, string messageName);
    }
}

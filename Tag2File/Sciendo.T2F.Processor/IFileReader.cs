namespace Sciendo.T2F.Processor
{
    public interface IFileReader<out T>
    {
        T ReadFile(string filePath);
    }
}

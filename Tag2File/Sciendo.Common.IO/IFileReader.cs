namespace Sciendo.Common.IO
{
    public interface IFileReader<out T>
    {
        T ReadFile(string filePath);
    }
}

namespace Sciendo.Common.IO
{
    public interface IFileReader<out T>
    {
        T Read(string filePath);
    }
}

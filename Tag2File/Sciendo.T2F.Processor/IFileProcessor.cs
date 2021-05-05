namespace Sciendo.T2F.Processor
{
    public interface IFileProcessor<in T>
    {
        string CalculateFileName(T input, string rootPath, string extension,string fileNamePattern, bool isPartOfCollection);
    }
}

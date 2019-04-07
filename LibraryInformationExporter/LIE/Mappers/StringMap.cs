using CsvHelper.Configuration;

namespace LIE.Mappers
{
    public class StringMap:ClassMap<string>
    {
        public StringMap()
        {
            Map(m => m).Name("FileWithError");
        }
    }
}

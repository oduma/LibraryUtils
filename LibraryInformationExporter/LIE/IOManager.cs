using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace LIE
{
    internal static class IoManager
    {
        public static List<T> ReadWithoutMapper<T>(string filePath) where T:class
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                return csv.GetRecords<T>().ToList();
            }

        }

        public static void WriteWithoutMapper<T>(IEnumerable<T> input, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer))
                csv.WriteRecords(input);
        }

        public static void WriteWithMapper<T, TMapper>(List<T> input, string filePath) where TMapper : ClassMap
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<TMapper>();
                csv.WriteRecords(input);
            }
        }
    }
}

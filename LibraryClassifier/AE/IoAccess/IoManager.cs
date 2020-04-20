using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AE.IoAccess
{
    internal static class IoManager
    {
        public static List<T> ReadWithMapper<T, TMapper>(string filePath) where T : class where TMapper : ClassMap
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                return csv.GetRecords<T>().ToList();
            }

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

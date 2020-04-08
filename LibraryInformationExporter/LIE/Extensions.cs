using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LIE.DataTypes;
using LIE.Mappers;

namespace LIE
{
    public static class Extensions
    {
        public static List<FileWithTags> WriteFile(this IEnumerable<FileWithTags> input, string filePath)
        {
            IoManager.WriteWithoutMapper(input, filePath);
            return input.ToList();

        }

        public static void WriteFile(this List<string> input, string filePath)
        {
            if (input.Count > 0)
                IoManager.WriteWithMapper<string, StringMap>(input, filePath);
        }

    }
}

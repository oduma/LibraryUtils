using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using LIE.DataTypes;

namespace LIE.Mappers
{
    public class ArtistTypeConvertor : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            Enum.TryParse(text, out ArtistType result);
            return result;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return ((ArtistType)value).ToString();
        }
    }
}

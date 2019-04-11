using System;
using System.Linq;
using CsvHelper.Configuration.Attributes;

namespace LIE.DataTypes
{
    public class Artist
    {

        [Name("artistID:ID(Artist)")]
        public Guid ArtistId { get; set; }
        [Name("name")]
        public string Name { get; set; }
        public ArtistType Type { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsComposer { get; set; }

        [Name(":LABEL")]
        public string ArtistLabels
        {
            get
            {
                return Type.ToString();
                //return $"{Type}{((IsComposer)?";Composer":"")}";
            }
            set
            {
                //var valueParts = value.Split(';');
                //if (valueParts.Length > 0)
                //{
                //    var composerString = valueParts.FirstOrDefault(s => s == "Composer");
                //    if (!string.IsNullOrEmpty(composerString))
                //        IsComposer = true;
                //    var nonComposerString = valueParts.FirstOrDefault(s => s != "Composer");
                //    if(!string.IsNullOrEmpty(nonComposerString))
                //    {
                //        ArtistType temp;
                //        if (ArtistType.TryParse(nonComposerString, true, out temp))
                //        {
                //            Type = temp;
                //        }
                //    }
                //}
                ;

            }
        }

    }
}

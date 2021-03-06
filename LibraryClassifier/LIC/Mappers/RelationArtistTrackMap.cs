﻿using CsvHelper.Configuration;
using LIC.DataTypes;

namespace LIC.Mappers
{
    internal sealed class RelationArtistTrackMap : ClassMap<RelationArtistTrack>
    {
        public RelationArtistTrackMap()
        {
            Map(m => m.ArtistId).Name(":START_ID(Artist)");
            Map(m => m.Year).Name("year");
            Map(m => m.TrackId).Name(":END_ID(Track)");
        }
    }
}
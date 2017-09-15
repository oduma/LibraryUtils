using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Clementine.DataAccess.DataTypes;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public static class Extensions
    {
        public static IEnumerable<RankedFile> FilterAndRank(this IEnumerable<ClementineTrack> inTracks,
            LoveTrack loveTrack)
        {
            foreach (var inTrack in inTracks)
            {
                var rankedFile = CalculateRankForFile(inTrack, loveTrack);
                if(rankedFile.Rank>0)
                    yield return rankedFile;
            }
        }

        private static RankedFile CalculateRankForFile(ClementineTrack inTrack, LoveTrack loveTrack)
        {
            return new RankedFile
            {
                FileName = inTrack.FileName,
                Rank=CalculateRank(inTrack.Title.ToLower(), loveTrack.Name.ToLower()) + CalculateRank(inTrack.Artist.ToLower(), loveTrack.Artist.Name.ToLower())
            };
        }

        private static int CalculateRank(string compareOne, string compareTwo)
        {
            if (string.IsNullOrEmpty(compareOne.Trim()))
                return 0;
            if (compareOne == compareTwo)
                return 2;
            if (compareOne.Contains(compareTwo))
                return 1;
            if (compareTwo.Contains(compareOne))
                return 1;
            return 0;
        }
    }
}

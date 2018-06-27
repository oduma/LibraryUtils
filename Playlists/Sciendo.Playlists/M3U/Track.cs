using System;
using System.IO;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;
using TagLib;

namespace Sciendo.Playlists.M3U
{
    internal class Track
    {

        private const string DurationSeparator = ",";

        private const string TitleSeparator = " - ";
        public string Location { get; set; }

        public string Title { get; set; }

        public string Creator { get; set; }

        public int Duration { get; set; }

        public Track()
        {
            
        }

        public Track(string trackTagContents)
        {
            if(string.IsNullOrEmpty(trackTagContents))
                throw new ArgumentNullException(nameof(trackTagContents));
            var workTrackContents = trackTagContents;
            var durationSeparatorPosition = workTrackContents.IndexOf(DurationSeparator, StringComparison.Ordinal);
            if (durationSeparatorPosition > 0)
            {
                try
                {
                    Duration = Convert.ToInt32(workTrackContents.Substring(0, durationSeparatorPosition).Trim());
                    workTrackContents = trackTagContents.Substring(durationSeparatorPosition + 1);
                }
                catch
                {
                    Duration = 0;
                }
            }

            var titleSeparatorPosition = workTrackContents.LastIndexOf(TitleSeparator, StringComparison.Ordinal);
            if (titleSeparatorPosition > 0)
            {
                Creator = workTrackContents.Substring(0, titleSeparatorPosition).Trim();
                Title = workTrackContents.Substring(titleSeparatorPosition + 1);
            }
        }

        public Track(IFile file, string path, string rootFolderPath)
        {
            var filePath = (string.IsNullOrEmpty(rootFolderPath))
                ? path
                : $"{rootFolderPath}{Path.DirectorySeparatorChar}{path}";

            if (file != null)
            {
                var tagFile = file.ReadTag(filePath);
                if(tagFile!=null)
                    FillInTheTag(tagFile);

            }
            Location = path;
        }

        private void FillInTheTag(TagLib.File tagFile)
        {
            var tag = tagFile.Tag;
            Creator = tag.FirstPerformer;
            Title = tag.Title;
            Duration = tagFile.Properties.Duration.Milliseconds;
        }

        public override string ToString()
        {
            string duration = (Duration==0)?string.Empty :$"{Duration}{DurationSeparator}";
            if(TrackHasTag())
                return $"{duration} {Creator}{TitleSeparator}{Title}{Environment.NewLine}{Location}";
            return $"{Environment.NewLine}{Location}";
        }


        internal bool TrackHasTag()
        {
            if (string.IsNullOrEmpty(Creator) && string.IsNullOrEmpty(Title))
                return false;
            return true;
        }


    }
}

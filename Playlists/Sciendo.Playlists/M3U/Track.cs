﻿using System;
using System.IO;
using Sciendo.Common.IO;
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

        public Track(IFileReader<Tag> tagFileReader, string file, string rootFolderPath)
        {
            var filePath = (string.IsNullOrEmpty(rootFolderPath))
                ? file
                : $"{rootFolderPath}{Path.DirectorySeparatorChar}{file}";

            if (tagFileReader != null)
                FillInTheTag(tagFileReader, filePath);
            Duration = 0;
            Location = file;
        }

        private void FillInTheTag(IFileReader<Tag> tagFileReader, string file)
        {
            var tag = tagFileReader.Read(file);
            Creator = tag.FirstPerformer;
            Title = tag.Title;
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LIE.DataTypes;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;

namespace LIE
{
    public class TagsProvider
    {
        public event EventHandler<TagProviderProgressEventArgs> Progress;
        public event EventHandler<TagProviderProgressEventArgs> ProgressWithError;
        private readonly FsStorage _fsStorage;
        private static bool _messageStop;

        public TagsProvider(FsStorage fsStorage)
        {
            _fsStorage = fsStorage;
        }

        public IEnumerable<FileWithTags> ScanPath(string folderPath, string[] extensions)
        {
            Progress?.Invoke(this, new TagProviderProgressEventArgs($"Starting scanning from folder {folderPath}..."));
            Progress?.Invoke(this,
                new TagProviderProgressEventArgs(
                    $"Detected {_fsStorage.Directory.GetFiles(folderPath, SearchOption.AllDirectories, extensions).Count()} files to be scanned ..."));

            return _fsStorage.Directory.GetFiles(folderPath, SearchOption.AllDirectories, extensions).AsParallel()
                .Select(GetFileWithTagsForFile).Where(t => t != null);
        }

        private FileWithTags GetFileWithTagsForFile(string filePath)
        {
            if (DateTime.Now.Second % 5 == 0 && !_messageStop)
            {
                Progress?.Invoke(this, new TagProviderProgressEventArgs($"Processing file: {filePath}"));
                _messageStop = true;
            }

            if (DateTime.Now.Second % 8 == 0)
            {
                _messageStop = false;
            }
            TagLib.Tag tag;
            try
            {
                tag = _fsStorage.File.ReadTag(filePath).Tag;
            }
            catch
            {
                ProgressWithError?.Invoke(this,new TagProviderProgressEventArgs(filePath));
                tag = null;
            }

            if (tag != null && !tag.IsEmpty)
            {
                return new FileWithTags
                {
                    AlbumArtists = tag.JoinedAlbumArtists,
                    Artists = tag.JoinedPerformers,
                    Composers = tag.JoinedComposers,
                    Album = tag.Album,
                    Title = tag.Title,
                    Year = tag.Year.ToString(),
                    Track = tag.Track.ToString(),
                    FilePath = filePath,
                    TrackId = Guid.NewGuid()
                };
            }

            return null;
        }



    }
}

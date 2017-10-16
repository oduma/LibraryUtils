using System;
using System.IO;
using Sciendo.Common.IO;
using Sciendo.Common.Serialization;
using Sciendo.Playlist.Handler.Contracts;

namespace Sciendo.Playlist.Persister
{
    public class PersisterProcessor
    {
        private readonly IFileEnumerator _fileEnumerator;
        private readonly IFileReader<string> _fileReader;
        private readonly IPlaylistHandlerFactory _playlistAnaliserFactory;
        private readonly string _sourceRoot;
        private readonly string _currentRoot;
        private readonly IContentWriter _fileWriter;

        public PersisterProcessor(IFileEnumerator fileEnumerator, 
            IFileReader<string> fileReader, 
            IPlaylistHandlerFactory playlistAnaliserFactory, 
            string sourceRoot, 
            string currentRoot, 
            IContentWriter fileWriter)
        {
            _fileEnumerator = fileEnumerator;
            _fileReader = fileReader;
            _playlistAnaliserFactory = playlistAnaliserFactory;
            _sourceRoot = sourceRoot.ToLower();
            _currentRoot = currentRoot.ToLower();
            _fileWriter = fileWriter;
        }

        public void Start(string path)
        {
            var files = _fileEnumerator.Get(path, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var playlistHandler = _playlistAnaliserFactory.GetHandler(Path.GetExtension(file));
                if (playlistHandler != null)
                {
                    var playlistRawContent = _fileReader.ReadFile(file);
                    if (!string.IsNullOrEmpty(playlistRawContent))
                    {
                        var targetDirectory = CreateTargetDirectory(file);
                        var playlist = playlistHandler.GetPlaylistItems(playlistRawContent);
                        CopyContent(targetDirectory, playlist);
                        var newPlaylistRawContent = playlistHandler.SetPlaylistItems(playlistRawContent, playlist);
                        File.WriteAllText(Path.Combine(targetDirectory,Path.GetFileName(file)),newPlaylistRawContent);
                    }
                }
            }
        }

        private void CopyContent(string targetDirectory, PlaylistItem[] playlistItems)
        {
            foreach (var playlistItem in playlistItems)
            {
                var sourceFile = playlistItem.TransformedFileName.ToLower().Replace(_sourceRoot, _currentRoot);
                var targetFile = Path.Combine(targetDirectory, Path.GetFileName(sourceFile));
                _fileWriter.Do(sourceFile,targetFile);
                playlistItem.TransformedFileName = Path.GetFileName(targetFile);
            }
        }

        private string CreateTargetDirectory(string playlistFileName)
        {
            var targetDirectory =
                $"{AppDomain.CurrentDomain.BaseDirectory}{Path.GetFileName(playlistFileName).Replace(Path.GetExtension(playlistFileName), "")}";
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);
            return targetDirectory;
        }


        public void Stop()
        {
        }

    }
}

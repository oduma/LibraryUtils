using System;
using System.IO;
using System.Linq;
using Sciendo.Common.IO;
using Sciendo.Common.Serialization;
using Sciendo.Playlist.Handler.Contracts;
using TagLib;
using File = System.IO.File;

namespace Sciendo.Playlist.Persister
{
    public class PersisterProcessor
    {
        private readonly IFileEnumerator _fileEnumerator;
        private readonly IFileReader<string> _textFileReader;
        private readonly IPlaylistHandlerFactory _playlistAnaliserFactory;
        private readonly string _sourceRoot;
        private readonly string _currentRoot;
        private readonly IContentWriter _fileWriter;
        private readonly IFileReader<Tag> _tagFileReader;
        private readonly PlaylistType _targetPlaylistType;

        public PersisterProcessor(IFileEnumerator fileEnumerator, 
            IFileReader<string> textFileReader, 
            IFileReader<Tag> tagFileReader,
            IPlaylistHandlerFactory playlistAnaliserFactory, 
            string sourceRoot, 
            string currentRoot, 
            IContentWriter fileWriter,
            PlaylistType targetPlaylistType)
        {
            _fileEnumerator = fileEnumerator;
            _textFileReader = textFileReader;
            _tagFileReader = tagFileReader;
            _playlistAnaliserFactory = playlistAnaliserFactory;
            _sourceRoot = sourceRoot.ToLower();
            _currentRoot = currentRoot.ToLower();
            _fileWriter = fileWriter;
            _targetPlaylistType = targetPlaylistType;
        }

        public event EventHandler<ProgressEventArgs> StartProcessing;
        public event EventHandler<ProgressEventArgs> StartProcessingFile;
        public event EventHandler<ProgressEventArgs> CopyContentToTarget;
        public event EventHandler<ProgressEventArgs> PlaylistCreated;   
        public void Start(string path)
        {
            if (Directory.Exists(path))
            {
                var files = _fileEnumerator.Get(path, SearchOption.TopDirectoryOnly);
                if (StartProcessing != null)
                    StartProcessing(this, new ProgressEventArgs(path,files.Count()));

                foreach (var file in files)
                {
                    ProcessFile(file);
                }
            }
            if (File.Exists(path))
            {
                if (StartProcessing != null)
                    StartProcessing(this, new ProgressEventArgs(path, 1));
                ProcessFile(path);
            }

        }

        private void ProcessFile(string file)
        {
            var inPlaylist = ReadPlaylist(file);
            if (inPlaylist != null)
            {
                if(StartProcessingFile!=null)
                    StartProcessingFile(this, new ProgressEventArgs(file,inPlaylist.Length));
                var targetDirectory = HandleContent(file, inPlaylist);
                CreateTargetPlaylist(inPlaylist, file, targetDirectory);
            }
            else
            {
                if(StartProcessingFile!=null)
                    StartProcessingFile(this, new ProgressEventArgs(file,0));
            }
        }

        private void CreateTargetPlaylist(PlaylistItem[] inPlaylist, string sourceFile, string targetDirectory)
        {
            var playlistHandler = _playlistAnaliserFactory.GetHandler(_targetPlaylistType.ToString().ToLower());
            var newPlaylistRawContent = playlistHandler.SetPlaylistItems(_tagFileReader, inPlaylist,targetDirectory);
            var targetFileName = $"{Path.GetFileNameWithoutExtension(sourceFile)}.{_targetPlaylistType.ToString().ToLower()}";
            File.WriteAllText(Path.Combine(targetDirectory, targetFileName), newPlaylistRawContent);
            if(PlaylistCreated!=null)
                PlaylistCreated(this, new ProgressEventArgs(targetFileName,1));
        }

        private string HandleContent(string file, PlaylistItem[] inPlaylist)
        {
            var targetDirectory = CreateTargetDirectory(file);
            var countFiles = CopyContent(targetDirectory, inPlaylist);
            if (CopyContentToTarget != null)
                CopyContentToTarget(this, new ProgressEventArgs(targetDirectory,countFiles));
            return targetDirectory;
        }

        private PlaylistItem[] ReadPlaylist(string file)
        {
            var playlistHandler = _playlistAnaliserFactory.GetHandler(Path.GetExtension(file));
            if (playlistHandler != null)
            {
                var playlistRawContent = _textFileReader.ReadFile(file);
                if (!string.IsNullOrEmpty(playlistRawContent))
                {
                    return playlistHandler.GetPlaylistItems(playlistRawContent);
                }
            }
            return null;
        }

        private int CopyContent(string targetDirectory, PlaylistItem[] playlistItems)
        {
            int fileCopyCount = 0;
            foreach (var playlistItem in playlistItems)
            {
                var sourceFile = (_sourceRoot.ToLower()==_currentRoot.ToLower())?
                    playlistItem.FileName:
                    playlistItem.FileName.ToLower().Replace(_sourceRoot, _currentRoot);
                var targetFile = Path.Combine(targetDirectory, Path.GetFileName(sourceFile));
                _fileWriter.Do(sourceFile,targetFile);
                playlistItem.FileName = Path.GetFileName(targetFile);
                fileCopyCount++;
            }
            return fileCopyCount;
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

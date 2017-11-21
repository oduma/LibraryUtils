using System;
using System.IO;
using System.Linq;
using Sciendo.Common.IO;
using Sciendo.Playlists;
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
        private readonly IFileWriter _textFileWriter;
        private readonly PlaylistType _targetPlaylistType;
        private readonly DeviceType _deviceType;

        public PersisterProcessor(IFileEnumerator fileEnumerator, 
            IFileReader<string> textFileReader, 
            IFileReader<Tag> tagFileReader,
            IFileWriter textFileWriter,
            IPlaylistHandlerFactory playlistAnaliserFactory, 
            string sourceRoot, 
            string currentRoot, 
            IContentWriter fileWriter,
            PlaylistType targetPlaylistType,
            DeviceType deviceType)
        {
            _fileEnumerator = fileEnumerator;
            _textFileReader = textFileReader;
            _tagFileReader = tagFileReader;
            _textFileWriter = textFileWriter;
            _playlistAnaliserFactory = playlistAnaliserFactory;
            _sourceRoot = sourceRoot.ToLower();
            _currentRoot = currentRoot.ToLower();
            _fileWriter = fileWriter;
            _deviceType = deviceType;
            _targetPlaylistType = _deviceType==DeviceType.Mobile ? PlaylistType.M3U : targetPlaylistType;
        }

        public event EventHandler<ProgressEventArgs> StartProcessing;
        public event EventHandler<ProgressEventArgs> StartProcessingFile;
        public event EventHandler<ProgressEventArgs> CopyContentToTarget;
        public event EventHandler<ProgressEventArgs> PlaylistCreated;   
        public void Start(string path)
        {
            if (Directory.Exists(path))
            {
                var files = _fileEnumerator.Get(path, SearchOption.TopDirectoryOnly).ToArray();
                StartProcessing?.Invoke(this, new ProgressEventArgs(path, files.Length));

                foreach (var file in files)
                {
                    ProcessFile(file);
                }
            }
            if (File.Exists(path))
            {
                StartProcessing?.Invoke(this, new ProgressEventArgs(path, 1));
                ProcessFile(path);
            }

        }

        private void ProcessFile(string file)
        {
            var inPlaylist = ReadPlaylist(file);
            if (inPlaylist != null)
            {
                StartProcessingFile?.Invoke(this, new ProgressEventArgs(file,inPlaylist.Length));
                var targetDirectory = HandleContent(file, inPlaylist);
                CreateTargetPlaylist(inPlaylist, file, targetDirectory);
            }
            else
            {
                StartProcessingFile?.Invoke(this, new ProgressEventArgs(file,0));
            }
        }

        private void CreateTargetPlaylist(PlaylistItem[] inPlaylist, string sourceFile, string targetDirectory)
        {
            var playlistHandler = _playlistAnaliserFactory.GetHandler(_targetPlaylistType.ToString().ToLower());

            var newPlaylistRawContent = playlistHandler.SetPlaylistItems((_deviceType!=DeviceType.Mobile)?_tagFileReader:null, inPlaylist,targetDirectory);
            var targetFileName = $"{Path.GetFileNameWithoutExtension(sourceFile)}.{_targetPlaylistType.ToString().ToLower()}";
            _textFileWriter.Write(newPlaylistRawContent, Path.Combine(targetDirectory, targetFileName));
            PlaylistCreated?.Invoke(this, new ProgressEventArgs(targetFileName,1));
        }

        private string HandleContent(string file, PlaylistItem[] inPlaylist)
        {
            var targetDirectory = CreateTargetDirectory(file);
            var countFiles = CopyContent(targetDirectory, inPlaylist);
            CopyContentToTarget?.Invoke(this, new ProgressEventArgs(targetDirectory,countFiles));
            return targetDirectory;
        }

        private PlaylistItem[] ReadPlaylist(string file)
        {
            var playlistHandler = _playlistAnaliserFactory.GetHandler(Path.GetExtension(file));
            if (playlistHandler != null)
            {
                var playlistRawContent = _textFileReader.Read(file);
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
                var targetFile = GetTargetFilePath(targetDirectory, sourceFile, ++fileCopyCount);
                _fileWriter.Do(sourceFile,targetFile);
                playlistItem.FileName = Path.GetFileName(targetFile);
            }
            return fileCopyCount;
        }

        private string GetTargetFilePath(string targetDirectory, string sourceFile, int indexInPlaylist)
        {
            var targetExtension = Path.GetExtension(sourceFile);

            var targetFileName = Path.GetFileNameWithoutExtension(sourceFile);
            if (_deviceType == DeviceType.Mobile)
                targetFileName = indexInPlaylist.ToString().PadLeft(3, '0');
            var targetFile = $"{targetFileName}{targetExtension}";
            return Path.Combine(targetDirectory, targetFile);
        }

        private string CreateTargetDirectory(string playlistFileName)
        {
            var fileName = Path.GetFileName(playlistFileName);
            if (fileName != null)
            {
                var targetDirectory =
                    $"{AppDomain.CurrentDomain.BaseDirectory}{fileName.Replace(Path.GetExtension(fileName), "")}";
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);
                return targetDirectory;
            }
           return playlistFileName;
        }


        public void Stop()
        {
        }

    }
}

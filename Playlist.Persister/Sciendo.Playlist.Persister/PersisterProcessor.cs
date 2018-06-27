using System;
using System.IO;
using System.Linq;
using Sciendo.Common.IO;
using Sciendo.Common.IO.MTP;
using Sciendo.Playlists;

namespace Sciendo.Playlist.Persister
{
    public class PersisterProcessor
    {
        private readonly IStorage _sourceStorage;
        private readonly IStorage _targetStorage;
        private readonly PlaylistType _targetPlaylistType;

        public PersisterProcessor(IStorage sourceStorage,
            IStorage targetStorage,
            PlaylistType targetPlaylistType)
        {
            _sourceStorage = sourceStorage;
            _targetStorage = targetStorage;
            _targetPlaylistType = (targetStorage is MtpStorage) ? PlaylistType.M3U : targetPlaylistType;
        }

        public event EventHandler<ProgressEventArgs> StartProcessing;
        public event EventHandler<ProgressEventArgs> StartProcessingFile;
        public event EventHandler<ProgressEventArgs> CopyContentToTarget;
        public event EventHandler<ProgressEventArgs> PlaylistCreated;   
        public void Start(string inPath, string outPath)
        {
            if (Directory.Exists(inPath))
            {
                var files = _sourceStorage.Directory.GetFiles(inPath, SearchOption.TopDirectoryOnly).ToArray();
                StartProcessing?.Invoke(this, new ProgressEventArgs(inPath, files.Length));

                foreach (var file in files)
                {
                    ProcessFile(file, outPath);
                }
            }
            if (File.Exists(inPath))
            {
                StartProcessing?.Invoke(this, new ProgressEventArgs(inPath, 1));
                ProcessFile(inPath, outPath);
            }

        }

        private void ProcessFile(string file, string outPath)
        {
            var inPlaylist = ReadPlaylist(file);
            if (inPlaylist != null)
            {
                StartProcessingFile?.Invoke(this, new ProgressEventArgs(file,inPlaylist.Length));
                var targetDirectory = HandleContent(file, inPlaylist, outPath);
                CreateTargetPlaylist(inPlaylist, file, targetDirectory);
            }
            else
            {
                StartProcessingFile?.Invoke(this, new ProgressEventArgs(file,0));
            }
        }

        private void CreateTargetPlaylist(PlaylistItem[] inPlaylist, string sourceFile, string targetDirectory)
        {
            var playlistHandler = PlaylistHandlerFactory.GetHandler(_targetPlaylistType.ToString().ToLower());

            var newPlaylistRawContent = playlistHandler.SetPlaylistItems(_targetStorage.File, inPlaylist,targetDirectory);
            var targetFileName = $"{Path.GetFileNameWithoutExtension(sourceFile)}.{_targetPlaylistType.ToString().ToLower()}";
            _targetStorage.File.WriteAllText(Path.Combine(targetDirectory, targetFileName), newPlaylistRawContent);
            PlaylistCreated?.Invoke(this, new ProgressEventArgs(targetFileName,1));
        }

        private string HandleContent(string file, PlaylistItem[] inPlaylist, string outPath)
        {
            var targetDirectory = CreateTargetDirectory(file, outPath);
            var countFiles = CopyContent(targetDirectory, inPlaylist);
            CopyContentToTarget?.Invoke(this, new ProgressEventArgs(targetDirectory,countFiles));
            return targetDirectory;
        }

        private PlaylistItem[] ReadPlaylist(string file)
        {
            var playlistHandler = PlaylistHandlerFactory.GetHandler(Path.GetExtension(file));
            if (playlistHandler != null)
            {
                var playlistRawContent = _sourceStorage.File.ReadAllText(file);
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
                var sourceFile = playlistItem.FileName;
                var targetFile = GetTargetFilePath(targetDirectory, sourceFile, ++fileCopyCount);
                StartProcessingFile?.Invoke(this, new ProgressEventArgs(targetFile, 0));
                _targetStorage.File.Create(targetFile,_sourceStorage.File.Read(sourceFile));
                playlistItem.FileName = Path.GetFileName(targetFile);
            }
            return fileCopyCount;
        }

        private void PersisterProcessor_StartProcessingFile(object sender, ProgressEventArgs e)
        {
            throw new NotImplementedException();
        }

        private string GetTargetFilePath(string targetDirectory, string sourceFile, int indexInPlaylist)
        {
            var targetExtension = Path.GetExtension(sourceFile);

            var targetFileName = Path.GetFileNameWithoutExtension(sourceFile);
            if (_targetStorage is MtpStorage)
                targetFileName = indexInPlaylist.ToString().PadLeft(3, '0');
            var targetFile = $"{targetFileName}{targetExtension}";
            return Path.Combine(targetDirectory, targetFile);
        }

        private string CreateTargetDirectory(string playlistFileName, string outPath)
        {
            var fileName = Path.GetFileName(playlistFileName);
            if (fileName != null)
            {
                var targetDirectory =
                    $"{outPath}{Path.DirectorySeparatorChar}{fileName.Replace(Path.GetExtension(fileName), "")}";
                if (!_targetStorage.Directory.Exists(targetDirectory))
                    _targetStorage.Directory.CreateDirectory(targetDirectory);
                return targetDirectory;
            }
           return playlistFileName;
        }


        public void Stop()
        {
        }

    }
}

using Sciendo.Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Appender
{
    public class PlaylistAppender : IPlaylistAppender
    {
        private readonly IPlaylistReader _sourcePlaylistReader;
        private readonly IPlaylistReader _targetPlaylistReader;
        private readonly IPlaylistTransformer _playlistTransformer;
        private readonly IPlaylistsCombiner _playlistsCombiner;
        private readonly IPlaylistWriter _playlistWriter;
        private readonly string[] _playlistExtensions = new[] { "m3u", "xspf" };

        public PlaylistAppender(
            IPlaylistReader sourcePlaylistReader, 
            IPlaylistReader targetPlaylistReader,
            IPlaylistTransformer playlistTransformer, 
            IPlaylistsCombiner playlistsCombiner, 
            IPlaylistWriter playlistWriter)
        {
            _sourcePlaylistReader = sourcePlaylistReader;
            _targetPlaylistReader = targetPlaylistReader;
            _playlistTransformer = playlistTransformer;
            _playlistsCombiner = playlistsCombiner;
            _playlistWriter = playlistWriter;
        }
        private void AppendOneFile(string fromPath, string fromMusicRootPath, string toPath, string toMusicRootPath)
        {
            PlaylistType fromPlaylistType = (PlaylistType)Enum.Parse(typeof(PlaylistType), Path.GetExtension(fromPath).Replace(".",""), true);
            PlaylistType toPlaylistType= (PlaylistType)Enum.Parse(typeof(PlaylistType), Path.GetExtension(toPath).Replace(".", ""), true);
            var sourcePlaylist = _sourcePlaylistReader.Read(fromPath, fromPlaylistType);
            var transformedSourcePlaylist = sourcePlaylist
                .Select(i => { _playlistTransformer.ChangeTheRootPath(i, fromMusicRootPath, toMusicRootPath); return i; });
            var targetPlaylist = _targetPlaylistReader.Read(toPath, toPlaylistType);
            _playlistWriter.Write(toPath, toPlaylistType, _playlistsCombiner.Append(targetPlaylist,transformedSourcePlaylist));
        }

        public void Append(string fromPath, string fromMusicRootPath, string toPath, string toMusicRootPath)
        {
            if (string.IsNullOrEmpty(fromPath))
            {
                Console.WriteLine("From Path empty not accepted");
                return;
            }
            if (string.IsNullOrEmpty(toPath))
            {
                Console.WriteLine("To Path empty not accepted");
                return;
            }
            if (_sourcePlaylistReader.Storage.Directory.Exists(fromPath) && _targetPlaylistReader.Storage.Directory.Exists(toPath))
            {
                foreach (var playlistFileName in _sourcePlaylistReader.Storage.Directory.GetFiles(fromPath, SearchOption.TopDirectoryOnly))
                {
                    var targetPlaylistFilename = GetTargetPlaylistFileName(
                        toPath,
                        playlistFileName,
                        _targetPlaylistReader.Storage);
                    if (!string.IsNullOrEmpty(targetPlaylistFilename))
                        AppendOneFile(playlistFileName, fromMusicRootPath, targetPlaylistFilename, toMusicRootPath);
                }

            }
            if (_sourcePlaylistReader.Storage.File.Exists(fromPath) && _targetPlaylistReader.Storage.File.Exists(toPath))
            {
                AppendOneFile(fromPath, fromMusicRootPath, toPath, toMusicRootPath);
            }
        }

        private string GetTargetPlaylistFileName(string toPath, string playlistFileName, IStorage outStorage)
        {
            foreach (var playListExtension in _playlistExtensions)
            {
                var outFileName = Path.Combine(toPath, Path.GetFileName(playlistFileName)).ToLower().Replace(Path.GetExtension(playlistFileName).ToLower(), playListExtension.ToLower());
                if (outStorage.File.Exists(outFileName))
                    return outFileName;
            }
            return null;
        }
    }
}

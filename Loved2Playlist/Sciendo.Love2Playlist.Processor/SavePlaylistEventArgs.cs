namespace Sciendo.Love2Playlist.Processor
{
    public class SavePlaylistEventArgs
    {
        public string ToFile { get; private set; }

        public SavePlaylistEventArgs(string toFile)
        {
            ToFile = toFile;
        }
    }
}
namespace Sciendo.Love2Playlist.Processor
{
    public class SavePlaylistEventArgs
    {
        public string ToFile { get; private set; }

        public int TotalCummulated { get; private set; }

        public SavePlaylistEventArgs(int totalCummulated, string toFile)
        {
            ToFile = toFile;
            TotalCummulated = totalCummulated;
        }
    }
}
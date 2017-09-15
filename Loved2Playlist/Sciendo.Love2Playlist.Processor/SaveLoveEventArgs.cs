namespace Sciendo.Love2Playlist.Processor
{
    public class SaveLoveEventArgs
    {
        public int SavedPage { get; private set; }

        public string ToFile { get; private set; }

        public int TotalCummulated { get; private set; }

        public SaveLoveEventArgs(int savePage, int totalCummulated, string saveFile)
        {
            SavedPage = savePage;
            ToFile = saveFile;
            TotalCummulated = totalCummulated;
        }
    }
}
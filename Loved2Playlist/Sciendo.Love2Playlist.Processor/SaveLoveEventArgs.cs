namespace Sciendo.Love2Playlist.Processor
{
    public class SaveLoveEventArgs
    {
        public int SavedPage { get; private set; }

        public string ToFile { get; private set; }

        public SaveLoveEventArgs(int savePage, string saveFile)
        {
            SavedPage = savePage;
            ToFile = saveFile;
        }
    }
}
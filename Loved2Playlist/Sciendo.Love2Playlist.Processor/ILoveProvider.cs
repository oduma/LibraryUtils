using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public interface ILoveProvider
    {
        LovePage GetPage(int currentLovedPage);
    }
}

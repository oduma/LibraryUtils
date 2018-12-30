using System.Collections.Generic;

namespace LIE
{
    internal class TrackWithFileEqualityComparer : IEqualityComparer<TrackWithFile>
    {
        public bool Equals(TrackWithFile x, TrackWithFile y)
        {
            if (string.IsNullOrEmpty(x.File) || string.IsNullOrEmpty(y.File))
                return false;
            if (x.File == y.File)
                return true;
            return false;
        }

        public int GetHashCode(TrackWithFile obj)
        {
            return obj.File.GetHashCode();
        }
    }
}
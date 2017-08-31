using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public class LoveCollection: ILoveCollection
    {
        public IEnumerator<LoveTrack> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(LoveTrack item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(LoveTrack item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(LoveTrack[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(LoveTrack item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; }
        public bool IsReadOnly { get; }
        public int IndexOf(LoveTrack item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, LoveTrack item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public LoveTrack this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Persist()
        {
            throw new NotImplementedException();
        }
    }
}

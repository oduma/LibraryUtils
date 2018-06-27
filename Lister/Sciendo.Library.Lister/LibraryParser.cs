using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.IO;

namespace Sciendo.Library.Lister
{
    public class LibraryParser
    {
        private readonly string _rootFolder;
        private readonly string[] _musicExtensions;
        private readonly Scope _includeScope;
        private readonly bool _includeSize;
        private readonly IDirectory _directory;

        public event EventHandler<ItemParsedEventArgs> ItemParsed; 

        public List<LibraryItem> ParseLibrary()
        {
            return ParseFolder(_rootFolder);
        }

        private List<LibraryItem> ParseFolder(string folder)
        {
            List<LibraryItem> libraryItems= new List<LibraryItem>(); 
            libraryItems.Add(CreateFolderLibraryItem(folder));
            libraryItems.AddRange(GetFileLibraryItemsInFolder(folder).Where(i=>i!=null));

            foreach (var folderInTheFolder in _directory.GetTopLevel(folder).OrderBy(s=>s, StringComparer.InvariantCultureIgnoreCase))
            {
                libraryItems.AddRange(ParseFolder(folderInTheFolder));
            }
            return libraryItems;
        }

        private IEnumerable<LibraryItem> GetFileLibraryItemsInFolder(string folder)
        {
            foreach (var fileInTheFolder in _directory.GetFiles(folder,SearchOption.TopDirectoryOnly).OrderBy(s => s, StringComparer.InvariantCultureIgnoreCase))
            {
                 yield return CreateFileLibraryItem(fileInTheFolder);
            }
        }

        private LibraryItem CreateFolderLibraryItem(string folder)
        {
            var libraryItem = new LibraryItem {ItemType = ItemType.Folder, Name = folder.Replace(_rootFolder, "")};
            ItemParsed?.Invoke(this, new ItemParsedEventArgs(libraryItem.ToString()));
            return libraryItem;
        }
        private LibraryItem CreateFileLibraryItem(string file)
        {
            var itemType = GetItemType(file);
            if (_includeScope == Scope.MusicOnly && itemType != ItemType.MusicFile)
                return null;
            var libraryItem= new LibraryItem { ItemType = itemType, Name = file.Replace(_rootFolder, ""), Size = (_includeSize)?GetItemSize(file):0 };
            ItemParsed?.Invoke(this, new ItemParsedEventArgs(libraryItem.ToString()));
            return libraryItem;
        }

        private long GetItemSize(string file)
        {
            return new FileInfo(file).Length;
        }

        private ItemType GetItemType(string file)
        {
            return (_musicExtensions.Contains(Path.GetExtension(file))) ? ItemType.MusicFile : ItemType.OtherFile;
        }

        public LibraryParser(string rootFolder, string[] musicExtensions, Scope includeScope, bool includeSize,IDirectory directory)
        {
            _musicExtensions = musicExtensions;
            _includeScope = includeScope;
            _includeSize = includeSize;
            _directory = directory;
            _rootFolder = (string.IsNullOrEmpty(rootFolder))?
                AppDomain.CurrentDomain.BaseDirectory:rootFolder;

        }
    }
}

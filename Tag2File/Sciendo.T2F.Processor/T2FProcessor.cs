using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;
using TagLib;

namespace Sciendo.T2F.Processor
{
    public class T2FProcessor
    {
        private readonly IStorage _storage;
        private readonly IFileProcessor<Tag> _tagFileProcessor;
        private List<string> _directoriesProcessed;
        private const string Various="Various";
        private const string Artists="Artists";
        private Dictionary<string, bool> _collectionDirectories;

        public T2FProcessor(IStorage storage, IFileProcessor<Tag> tagFileProcessor)
        {
            _storage = storage;
            _tagFileProcessor = tagFileProcessor;
        }

        public void Start(string path, string[] extensions, string fileNamePattern, string fileNamePatternCollection, ActionType actionType)
        {
            _directoriesProcessed=new List<string>();
            _collectionDirectories= new Dictionary<string, bool>();
            var files = _storage.Directory.GetFiles(path,SearchOption.AllDirectories, extensions);
            foreach (var file in files)
            {
                var tag = _storage.File.ReadTag(file).Tag;
                if (!(string.IsNullOrEmpty(tag?.Album) || string.IsNullOrEmpty(tag.Title)))
                {
                    
                    var fileExtension = Path.GetExtension(file);
                    var newFileName = _tagFileProcessor.CalculateFileName(tag,path,fileExtension,
                        (IsPartOfCollection(file, extensions))?fileNamePatternCollection:fileNamePattern);
                    _storage.File.Create(newFileName,_storage.File.Read(file));
                    if(actionType==ActionType.Move)
                        _storage.File.Delete(file);
                    if (_directoriesProcessed.All(d => d != Path.GetDirectoryName(file)))
                    {
                        ProcessNonMusicContents(Path.GetDirectoryName(file), Path.GetDirectoryName(newFileName),
                            extensions, actionType);
                    }
                }
            }
            CleanupEmptyDirectories(path);
        }

        private void CleanupEmptyDirectories(string path)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                CleanupEmptyDirectories(directory);
                if (!_storage.Directory.GetFiles(directory,SearchOption.AllDirectories).Any() &&
                    !_storage.Directory.GetTopLevel(directory).Any())
                {
                    _storage.Directory.Delete(directory, false);
                }
            }
        }

        private void ProcessNonMusicContents(string sourceDirectoryName, string destinationDirectoryName, string[] extensions,ActionType actionType)
        {
            var allFilesinTopFolder = _storage.Directory.GetFiles(sourceDirectoryName, SearchOption.AllDirectories).ToArray();

            var nonMusicInTopFolder = allFilesinTopFolder.Where(f=>!extensions.Contains(Path.GetExtension(f)));
            foreach (var nonMusicFile in nonMusicInTopFolder)
            {
                _storage.File.Create(nonMusicFile.Replace(sourceDirectoryName, destinationDirectoryName),
                    _storage.File.Read(nonMusicFile));
                if(actionType==ActionType.Move)
                    _storage.File.Delete(nonMusicFile);
            }
            _directoriesProcessed.Add(sourceDirectoryName);
        }

        private bool IsPartOfCollection(string filePath, string[] extensions)
        {
            var parentDirectory = Path.GetDirectoryName(filePath);
            if(parentDirectory==null)
                throw new Exception("Something wrong.");
            if (_collectionDirectories.ContainsKey(parentDirectory))
                return _collectionDirectories[parentDirectory];
            Tag tagProcessed = _storage.File.ReadTag(filePath).Tag;
            if (tagProcessed.AlbumArtists.Any(aa => aa.Contains(Various) || aa.Contains(Artists)))
            {
                _collectionDirectories.Add(parentDirectory,true);
                return true;
            }
            var tags =
                _storage.Directory.GetFiles(parentDirectory, SearchOption.AllDirectories, extensions)
                    .Select(fn => _storage.File.ReadTag(fn).Tag);
            if (tags.SelectMany(t => t.Performers).Distinct().Count() > 1)
            {
                _collectionDirectories.Add(parentDirectory,true);
                return true;
            }
            _collectionDirectories.Add(parentDirectory,false);
            return false;
        }

        public void Stop()
        {
        }
    }
}

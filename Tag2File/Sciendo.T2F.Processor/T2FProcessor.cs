using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.IO;
using TagLib;

namespace Sciendo.T2F.Processor
{
    public class T2FProcessor
    {
        private readonly IFileEnumerator _fileEnumerator;
        private readonly IDirectoryEnumerator _directoryEnumerator;
        private readonly IFileReader<TagLib.File> _tagFileReader;
        private readonly IFileProcessor<Tag> _tagFileProcessor;
        private readonly IContentWriter _fileWriter;
        private List<string> _directoriesProcessed;
        private const string Various="Various";
        private const string Artists="Artists";
        private Dictionary<string, bool> _collectionDirectories;

        public T2FProcessor(IFileEnumerator fileEnumerator,IDirectoryEnumerator directoryEnumerator,
            IFileReader<TagLib.File> tagFileReader, IFileProcessor<Tag> tagFileProcessor, IContentWriter fileWriter)
        {
            _fileEnumerator = fileEnumerator;
            _directoryEnumerator = directoryEnumerator;
            _tagFileReader = tagFileReader;
            _tagFileProcessor = tagFileProcessor;
            _fileWriter = fileWriter;
        }

        public void Start(string path, string[] extensions, string fileNamePattern, string fileNamePatternCollection)
        {
            _directoriesProcessed=new List<string>();
            _collectionDirectories= new Dictionary<string, bool>();
            var files = _fileEnumerator.Get(path,SearchOption.AllDirectories, extensions);
            foreach (var file in files)
            {
                var tag = _tagFileReader.Read(file).Tag;
                if (!(string.IsNullOrEmpty(tag?.Album) || string.IsNullOrEmpty(tag.Title)))
                {
                    
                    var fileExtension = Path.GetExtension(file);
                    var newFileName = _tagFileProcessor.CalculateFileName(tag,path,fileExtension,
                        (IsPartOfCollection(file, extensions))?fileNamePatternCollection:fileNamePattern);
                    _fileWriter.Do(file,newFileName);
                    if (_directoriesProcessed.All(d => d != Path.GetDirectoryName(file)))
                    {
                        ProcessNonMusicContents(Path.GetDirectoryName(file),Path.GetDirectoryName(newFileName), extensions);
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
                if (!_fileEnumerator.Get(directory,SearchOption.AllDirectories).Any() &&
                    !_directoryEnumerator.GetTopLevel(directory).Any())
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        private void ProcessNonMusicContents(string sourceDirectoryName, string destinationDirectoryName, string[] extensions)
        {
            var allFilesinTopFolder = _fileEnumerator.Get(sourceDirectoryName, SearchOption.TopDirectoryOnly).ToArray();

            var nonMusicInTopFolder = allFilesinTopFolder.Where(f=>!extensions.Contains(Path.GetExtension(f)));
            foreach (var nonMusicFile in nonMusicInTopFolder)
            {
                _fileWriter.Do(nonMusicFile,nonMusicFile.Replace(sourceDirectoryName,destinationDirectoryName));
            }
            var childDirectories = _directoryEnumerator.GetTopLevel(sourceDirectoryName);
            foreach (var childDirectory in childDirectories)
            {
                var childDirectoryParts = childDirectory.Split(Path.DirectorySeparatorChar);
                       
                _fileWriter.Do(childDirectory,$"{destinationDirectoryName}{Path.DirectorySeparatorChar}{childDirectoryParts[childDirectoryParts.Length-1]}");
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
            Tag tagProcessed = _tagFileReader.Read(filePath).Tag;
            if (tagProcessed.AlbumArtists.Any(aa => aa.Contains(Various) || aa.Contains(Artists)))
            {
                _collectionDirectories.Add(parentDirectory,true);
                return true;
            }

            var tags = ((IFilesReader<Tag>)_tagFileReader).Read(_fileEnumerator.Get(parentDirectory, SearchOption.AllDirectories, extensions));
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

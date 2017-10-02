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
        private readonly IFileReader<Tag> _tagFileReader;
        private readonly IFileProcessor<Tag> _tagFileProcessor;
        private readonly IContentWriter _fileWriter;
        private List<string> _directoriesProcessed;
        private const string Various="Various";
        private const string Artists="Artists";

        public T2FProcessor(IFileEnumerator fileEnumerator,IDirectoryEnumerator directoryEnumerator,IFileReader<Tag> tagFileReader, IFileProcessor<Tag> tagFileProcessor, IContentWriter fileWriter)
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
            var files = _fileEnumerator.Get(path,SearchOption.AllDirectories, extensions);
            foreach (var file in files)
            {
                var tag = _tagFileReader.ReadFile(file);
                var fileExtension = Path.GetExtension(file);
                var newFileName = _tagFileProcessor.CalculateFileName(tag,path,fileExtension,(IsPartOfCollection(tag))?fileNamePatternCollection:fileNamePattern);
                _fileWriter.Do(file,newFileName);
                if (_directoriesProcessed.All(d => d != Path.GetDirectoryName(file)))
                {
                    ProcessNonMusicContents(Path.GetDirectoryName(file),Path.GetDirectoryName(newFileName), extensions);
                }
            }
            CleanupEmptyDirectories(path);
        }

        private void CleanupEmptyDirectories(string path)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                CleanupEmptyDirectories(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
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
                var childDirectoryParts = childDirectory.Split(new[] {Path.DirectorySeparatorChar});
                       
                _fileWriter.Do(childDirectory,$"{destinationDirectoryName}{Path.DirectorySeparatorChar}{childDirectoryParts[childDirectoryParts.Length-1]}");
            }
            _directoriesProcessed.Add(sourceDirectoryName);
        }

        private bool IsPartOfCollection(Tag tag)
        {
            return tag.AlbumArtists.Any(aa => aa.Contains(Various) || aa.Contains(Artists));
        }

        public void Stop()
        {
        }
    }
}

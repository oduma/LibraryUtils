using System;
using System.Collections.Generic;
using System.IO;
using Sciendo.Common.IO;
using Sciendo.Common.IO.MTP;

namespace Sciendo.Playlist.Translator
{
    public class BulkTranslator:TranslatorBase, IBulkTranslator
    {
        private readonly string _inPath;
        private readonly string[] _extensions;
        private readonly string _outPath;
        private readonly IStorage _sourceStorage;
        private readonly IStorage _targetStorage;

        public BulkTranslator(string inPath, string[] extensions, 
            string outPath, IStorage sourceStorage, IStorage targetStorage)
        {
            _inPath = inPath;
            _extensions = extensions;
            _outPath = outPath;
            _sourceStorage = sourceStorage;
            _targetStorage = targetStorage;
        }
        public void Start(Dictionary<string, string>  findReplaceParams)
        {
            if(findReplaceParams==null || findReplaceParams.Keys.Count<1)
                throw new ArgumentNullException(nameof(findReplaceParams));

            if (Directory.Exists(_inPath))
            {
                if (!Directory.Exists(_outPath) || string.Equals(_inPath,_outPath,StringComparison.InvariantCultureIgnoreCase))
                {
                    TranslateToSameDirectory(findReplaceParams);
                }
                TranslateToDifferentDirectory(findReplaceParams);
            }
            else
            {
                if(string.IsNullOrEmpty(_outPath))
                {
                    var translatedFileName =
                        $"{Path.GetDirectoryName(_inPath)}{Path.DirectorySeparatorChar}translated_{Path.GetFileName(_inPath)}";
                    TranslateFile(_sourceStorage,_sourceStorage,_inPath,translatedFileName, findReplaceParams);
                }
                else if(_targetStorage.Directory.Exists(_outPath))
                {
                    var inPathFileName = Path.GetFileName(_inPath);
                    var translatedFileName = $"{_outPath}{Path.DirectorySeparatorChar}{inPathFileName}";
                    TranslateFile(_sourceStorage,_targetStorage,_inPath, translatedFileName, findReplaceParams);
                }
                else if(Path.GetExtension(_inPath).ToLower()!=Path.GetExtension(_outPath).ToLower())
                {
                    var translateFileName = _outPath.Replace(Path.GetExtension(_outPath), Path.GetExtension(_inPath));
                    TranslateFile(_sourceStorage,_targetStorage,_inPath, translateFileName, findReplaceParams);
                }
                else
                {
                    TranslateFile(_sourceStorage,_targetStorage,_inPath, _outPath, findReplaceParams);
                }
            }
        }

        private void TranslateToDifferentDirectory(Dictionary<string, string> replacementVariants)
        {
            foreach (var file in _sourceStorage.Directory.GetFiles(_inPath, SearchOption.TopDirectoryOnly, _extensions))
            {
                var translatedFileName =
                    $"{_outPath}{Path.DirectorySeparatorChar}{Path.GetFileName(file)}";
                TranslateFile(_sourceStorage, _targetStorage, file, translatedFileName, replacementVariants);
            }
            PathTranslated?.Invoke(this, new PathEventArgs(_inPath));
        }

        private void TranslateFile(IStorage fromStorage, IStorage toStorage, string fromFile, string toFile,Dictionary<string, string> replacementVariants)
        {

            var contents = fromStorage.File.ReadAllText(fromFile);
            contents = PerformOneTranslation(replacementVariants, contents);
            
            toStorage.File.WriteAllText(contents,toFile);
            PathTranslated?.Invoke(this,new PathEventArgs(fromFile));
        }

        private void TranslateToSameDirectory(Dictionary<string, string> replacementVariants)
        {
            foreach (var file in _sourceStorage.Directory.GetFiles(_inPath, SearchOption.TopDirectoryOnly,_extensions))
            {
                var translatedFileName =
                    $"{Path.GetDirectoryName(file)}{Path.DirectorySeparatorChar}translated_{Path.GetFileName(file)}";
                TranslateFile(_sourceStorage,_sourceStorage, file,translatedFileName, replacementVariants);
            }
            PathTranslated?.Invoke(this, new PathEventArgs(_inPath));
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<PathEventArgs> PathTranslated;
    }
}

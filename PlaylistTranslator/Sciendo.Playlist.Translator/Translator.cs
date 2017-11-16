using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySynch.Q.Common.Contracts;
using Sciendo.Common.IO;
using Sciendo.Playlists;

namespace Sciendo.Playlist.Translator
{
    public class Translator:ITranslator
    {
        private readonly string _inPath;
        private readonly string[] _extensions;
        private readonly string _outPath;
        private readonly string _fromText;
        private readonly string _toText;
        private readonly IFileEnumerator _fileEnumerator;
        private readonly IFileReader<string> _textFileReader;
        private readonly IFileWriter _textFileWriter;

        public Translator(string inPath, string[] extensions, 
            string outPath, 
            string fromText,
            string toText,
            IFileEnumerator fileEnumerator, IFileReader<string> textFileReader, IFileWriter textFileWriter)
        {
            if(string.IsNullOrEmpty(inPath))
                throw  new ArgumentException(nameof(inPath));
            if(string.IsNullOrEmpty(fromText))
                throw new ArgumentException(nameof(fromText));
            _inPath = inPath;
            _extensions = extensions;
            _outPath = outPath;
            _fromText = fromText;
            _toText = toText;
            _fileEnumerator = fileEnumerator;
            _textFileReader = textFileReader;
            _textFileWriter = textFileWriter;
        }
        public void Start()
        {
            var replacementVariants = BuildReplacementVariants(_fromText,_toText);
            if (Directory.Exists(_inPath))
            {
                if (!Directory.Exists(_outPath) || string.Equals(_inPath,_outPath,StringComparison.InvariantCultureIgnoreCase))
                {
                    TranslateToSameDirectory(replacementVariants);
                }
                TranslateToDifferentDirectory(replacementVariants);
            }
            else
            {
                if(string.IsNullOrEmpty(_outPath))
                {
                    var translatedFileName =
                        $"{Path.GetDirectoryName(_inPath)}{Path.DirectorySeparatorChar}translated_{Path.GetFileName(_inPath)}";
                    TranslateFile(_inPath,translatedFileName, replacementVariants);
                }
                else if(Directory.Exists(_outPath))
                {
                    var inPathFileName = Path.GetFileName(_inPath);
                    var translatedFileName = $"{_outPath}{Path.DirectorySeparatorChar}{inPathFileName}";
                    TranslateFile(_inPath, translatedFileName, replacementVariants);
                }
                else if(Path.GetExtension(_inPath).ToLower()!=Path.GetExtension(_outPath).ToLower())
                {
                    var translateFileName = _outPath.Replace(Path.GetExtension(_outPath), Path.GetExtension(_inPath));
                    TranslateFile(_inPath, translateFileName, replacementVariants);
                }
                else
                {
                    TranslateFile(_inPath, _outPath, replacementVariants);
                }
            }
        }

        private void TranslateToDifferentDirectory(Dictionary<string, string> replacementVariants)
        {
            foreach (var file in _fileEnumerator.Get(_inPath, SearchOption.TopDirectoryOnly, _extensions))
            {
                var translatedFileName =
                    $"{_outPath}{Path.DirectorySeparatorChar}{Path.GetFileName(file)}";
                TranslateFile(file, translatedFileName, replacementVariants);
            }
            PathTranslated?.Invoke(this, new PathEventArgs(_inPath));
        }

        private Dictionary<string, string> BuildReplacementVariants(string originalFromText, string originalToText)
        {
            
            var replacementVariants = new Dictionary<string, string>();
            replacementVariants.Add(originalFromText, originalToText);
            return replacementVariants;
        }

        private void TranslateFile(string fromFile, string toFile,Dictionary<string, string> replacementVariants)
        {
            var contents = _textFileReader.Read(fromFile);
            contents = PerformOneTranslation(replacementVariants, contents);
            _textFileWriter.Write(contents,toFile);
            PathTranslated?.Invoke(this,new PathEventArgs(fromFile));
        }

        private static string PerformOneTranslation(Dictionary<string, string> replacementVariants, string contents)
        {
            foreach (var fromReplacement in replacementVariants.Keys)
            {
                contents = contents.Replace(fromReplacement, replacementVariants[fromReplacement]);
            }
            return contents;
        }

        private void TranslateToSameDirectory(Dictionary<string, string> replacementVariants)
        {
            foreach (var file in _fileEnumerator.Get(_inPath, SearchOption.TopDirectoryOnly,_extensions))
            {
                var translatedFileName =
                    $"{Path.GetDirectoryName(file)}{Path.DirectorySeparatorChar}translated_{Path.GetFileName(file)}";
                TranslateFile(file,translatedFileName, replacementVariants);
            }
            PathTranslated?.Invoke(this, new PathEventArgs(_inPath));
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<PathEventArgs> PathTranslated;

        public TransferMessage Translate(TransferMessage inMessage, params object[] additionalParameters)
        {
            if(inMessage.BodyType!=BodyType.Text)
                return inMessage;
            if(additionalParameters==null || additionalParameters.Length<2)
                return inMessage;
            var from = (string)additionalParameters[0];
            var to = (string)additionalParameters[1];
            inMessage.Body = PerformOneTranslation(BuildReplacementVariants(from, to), (string) inMessage.Body);
            return inMessage;

        }
    }
}

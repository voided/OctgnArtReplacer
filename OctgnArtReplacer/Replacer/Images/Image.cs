using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OctgnArtReplacer.Replacer.Images
{
    class Image
    {
        private readonly string _filePath;

        private string _cardName;
        private int? _multiverseId;

        private readonly static Regex cardRegex = new Regex(@"(?<card>.*?)(?:\.(?<art>.*))?\.\w*", RegexOptions.Compiled);


        public Image(string filePath)
        {
            _filePath = filePath;

            DetermineCardName(Path.GetFileName(_filePath));
        }


        public string CardName => _cardName;
        public int? MultiverseId => _multiverseId;
        public string FilePath => _filePath;


        void DetermineCardName(string fileName)
        {
            Match m = cardRegex.Match(fileName);

            if (m.Success)
            {
                _cardName = m.Groups["card"].Value;

                int multiId;
                if (int.TryParse(_cardName, out multiId))
                {
                    // if the card name was able to get parsed as an int, it's most likely a multiverse id
                    _multiverseId = multiId;
                }
            }
        }

        public override string ToString()
        {
            if (_multiverseId != null)
            {
                return $"({MultiverseId})";
            }

            return $"{CardName}";
        }
    }
}

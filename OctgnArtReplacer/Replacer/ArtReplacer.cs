using OctgnArtReplacer.Octgn;
using OctgnArtReplacer.Octgn.Mtg;
using OctgnArtReplacer.Octgn.Schema;
using OctgnArtReplacer.Octgn.SchemaExtensions;
using OctgnArtReplacer.Replacer.Images;
using OctgnArtReplacer.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctgnArtReplacer.Replacer
{
    class ArtReplacer
    {
        private readonly string _imageSourcePath;
        private readonly string _imageOutputPath;
        private readonly string _setName;

        private readonly OctgnContext _octgnContext;
        private readonly ImageSource _imageSource;

        private IEnumerable<Image> _images;
        private Set _set;


        public ArtReplacer(string imageSourcePath, string imageOutputPath, string setName)
        {
            _imageSourcePath = imageSourcePath;
            _imageOutputPath = imageOutputPath;
            _setName = setName;

            _octgnContext = new MtgContext(); // TODO: support more than mtg
            _imageSource = new ImageSource(_imageSourcePath);
        }

        public async Task<VerificationResult> Verify()
        {
            if (!Directory.Exists(_imageSourcePath))
            {
                Log.Error("Error: Unable to find image source path");
                return VerificationResult.Fail;
            }

            if (!_octgnContext.IsOctgnInstalled)
            {
                Log.Error("Error: OCTGN is not installed");
                return VerificationResult.Fail;
            }

            if (!_octgnContext.IsInstalled)
            {
                Log.Error("Error: Game definition is not installed");
                return VerificationResult.Fail;
            }

            var setList = await _octgnContext.GetSetsAsync();
            _set = setList
                .FirstOrDefault(s => string.Equals(s.Name, _setName, StringComparison.OrdinalIgnoreCase));
            
            if (_set == null)
            {
                Log.Error($"Error: Unable to find set with the name \"{_setName}\"");
                Log.Info("Available Sets:");

                foreach (var set in setList.OrderBy(s => s.Name))
                {
                    Log.Info($"  {set.Name}");
                }

                return VerificationResult.Fail;
            }

            _images = await _imageSource.GetImagesAsync();

            bool hasWarning = false,
                 hasError = false;

            var hasNewArtMap = _set.Cards
                .ToDictionary(k => k, e => false);

            foreach (Image image in _images)
            {
                bool lookingByMultiverseId = image.MultiverseId != null;
                IEnumerable<Card> cards = FindCardsForImage(image);

                if (!cards.Any())
                {
                    if (lookingByMultiverseId)
                    {
                        Log.Warning($"Unable to find card for image ({image.MultiverseId}) in the set \"{_set.Name}\"");
                    }
                    else
                    {
                        Log.Warning($"Unable to find card for image \"{image.CardName}\" in the set \"{_set.Name}\"");
                    }

                    hasWarning = true;
                    continue;
                }
                else if (cards.Count() > 1)
                {
                    Log.Error($"Error: Image \"{image.CardName}\" matches multiple cards in the set \"{_set.Name}\". Please rename the image to \"<multiverseid>.<size>.extension\"");

                    hasError = true;
                    continue;
                }

                hasNewArtMap[cards.FirstOrDefault()] = true;
            }

            if (hasError)
                return VerificationResult.Fail;

            var missedCards = hasNewArtMap
                .Where(kvp => kvp.Value == false)
                .Select(kvp => kvp.Key);

            foreach (Card card in missedCards)
            {
                Log.Warning($"Card \"{card.Name}\" has no associated art in the source directory!");
                hasWarning = true;
            }

            if (hasWarning)
                return VerificationResult.Warn;
            

            return VerificationResult.Ok;
        }


        private IEnumerable<Card> FindCardsForImage(Image image)
        {
            if (image.MultiverseId != null)
            {
                Card card = _set.GetCardByMultiverseId(image.MultiverseId.Value);

                if (card == null)
                    return Enumerable.Empty<Card>();

                return new[] { card };
            }

            return _set.GetCardsByName(image.CardName);
        }


        public async Task Run()
        {
            var imageOutput = new ImageOutput(_imageOutputPath, _set);
            await imageOutput.Init();

            var cardMapping = new Dictionary<Image, Card>();

            foreach (var image in _images)
            {
                cardMapping[image] = FindCardsForImage(image).First();
            }

            await imageOutput.Copy(cardMapping);
        }
    }
}

using OctgnArtReplacer.Octgn.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctgnArtReplacer.Replacer.Images
{
    class ImageOutput
    {
        private readonly string _path;
        private readonly Set _set;

        public ImageOutput(string path, Set set)
        {
            _path = path;
            _set = set;
        }

 
        public Task Init()
        {
            return Task.Run(() =>
            {
                if (!Directory.Exists(FullOutputPath))
                {
                    Directory.CreateDirectory(FullOutputPath);
                }
            });
        }


        public Task Copy(Dictionary<Image, Card> imageToCardMapping)
        {
            var copyTasks = new List<Task>();

            foreach (var mapping in imageToCardMapping)
            {
                Image image = mapping.Key;
                Card card = mapping.Value;

                string extension = Path.GetExtension(image.FilePath);
                string newImageFile = $"{card.Id}{extension}";

                string newImagePath = Path.Combine(CardsOutputPath, newImageFile);

                // todo: this will likely just exhaust the threadpool, but yolo
                copyTasks.Add(Task.Run(() => File.Copy(image.FilePath, newImagePath, overwrite: true)));
            }

            return Task.WhenAll(copyTasks);
        }


        private string CardsOutputPath => Path.Combine(_path, _set.Id.ToString(), "Cards");
        private string FullOutputPath => Path.Combine(CardsOutputPath, "Proxies");
    }
}

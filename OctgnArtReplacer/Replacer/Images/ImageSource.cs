using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctgnArtReplacer.Replacer.Images
{
    class ImageSource
    {
        private readonly string _path;

        public ImageSource(string path)
        {
            _path = path;
        }


        public Task<IEnumerable<Image>> GetImagesAsync()
        {
            return Task.Run<IEnumerable<Image>>(() =>
            {
                var images = new List<Image>();

                var directoryInfo = new DirectoryInfo(_path);

                foreach (FileInfo file in directoryInfo.EnumerateFiles())
                {
                    var image = new Image(file.FullName);
                    images.Add(image);
                }

                return images;
            });
        }
    }
}

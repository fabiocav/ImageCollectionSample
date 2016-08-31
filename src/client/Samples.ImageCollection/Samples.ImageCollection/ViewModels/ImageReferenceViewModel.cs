using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samples.ImageCollection.Model;
using Samples.ImageCollection.Services;
using Xamarin.Forms;

namespace Samples.ImageCollection.ViewModels
{
    public class ImageReferenceViewModel
    {
        private readonly IFileHelper _fileHelper;
        private readonly ImageReference _imageReference;

        public ImageReferenceViewModel(ImageReference imageReference, IFileHelper fileHelper)
        {
            _imageReference = imageReference;
            _fileHelper = fileHelper;
        }

        public string Id => _imageReference.Id;

        public ImageSource ImageSource
        {
            get
            {
                if (_imageReference.FileName == null)
                {
                    // TODO: Placeholder
                    return null;
                }

                string path = _fileHelper.GetLocalFilePath(_imageReference.Id, _imageReference.FileName);

                // TODO: Cache this...
                if (path.StartsWith("http"))
                {
                    return ImageSource.FromUri(new Uri(path));
                }
                else
                {
                    return ImageSource.FromFile(path);
                }
            }
        }
    }
}

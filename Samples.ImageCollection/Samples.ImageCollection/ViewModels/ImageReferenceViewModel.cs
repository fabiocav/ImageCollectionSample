using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samples.ImageCollection.Model;
using Xamarin.Forms;

namespace Samples.ImageCollection.ViewModels
{
    public class ImageReferenceViewModel
    {
        private readonly ImageReference _imageReference;

        public ImageReferenceViewModel(ImageReference imageReference)
        {
            _imageReference = imageReference;
        }

        public string Id => _imageReference.Id;

        public ImageSource ImageSource
        {
            get
            {
                if (_imageReference.Uri.StartsWith("http"))
                {
                    return ImageSource.FromUri(new Uri(_imageReference.Uri));
                }
                else
                {
                    return ImageSource.FromFile(_imageReference.Uri);
                }
            }
        }
    }
}

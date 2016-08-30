using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samples.ImageCollection.Services;
using Windows.Storage;
using Windows.Storage.Pickers;

[assembly: Xamarin.Forms.Dependency(typeof(Samples.ImageCollection.UWP.ImageSelector))]
namespace Samples.ImageCollection.UWP
{
    public class ImageSelector : IImageSelector
    {
        public async Task<string> SelectImageAsync()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();

            file = await file?.CopyAsync(ApplicationData.Current.LocalFolder, Guid.NewGuid().ToString());
            
            return file?.Path;
        }
    }
}

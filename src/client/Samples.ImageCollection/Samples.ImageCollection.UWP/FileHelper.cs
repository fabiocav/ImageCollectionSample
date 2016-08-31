using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Files.Sync;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Samples.ImageCollection.Services;
using Windows.Storage;
using Windows.Storage.Pickers;

[assembly: Xamarin.Forms.Dependency(typeof(Samples.ImageCollection.UWP.FileHelper))]
namespace Samples.ImageCollection.UWP
{
    public class FileHelper : IFileHelper
    {
        public async Task<string> SelectImageAsync(string categoryId)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();

            IStorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(categoryId, CreationCollisionOption.OpenIfExists);
            
            file = await file?.CopyAsync(folder, Guid.NewGuid().ToString() + Path.GetExtension(file.Name));
            
            return file?.Path;
        }

        public async Task DownloadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file, string targetPath)
        {
            await table.DownloadFileAsync(file, targetPath);
        }

        public async Task UploadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file, string filePath)
        {
            await table.UploadFileAsync(file, filePath);
        }

        public IMobileServiceFileDataSource GetMobileServiceDataSource(string filePath)
        {
            return new PathMobileServiceFileDataSource(filePath);
        }

        public string GetLocalFilePath(string itemId, string fileName)
        {
            string recordFilesPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, itemId);

            if (!Directory.Exists(recordFilesPath))
            {
                Directory.CreateDirectory(recordFilesPath);
            }

            return Path.Combine(recordFilesPath, fileName);
        }

        public void DeleteLocalFile(MobileServiceFile file)
        {
            string localPath = GetLocalFilePath(file.ParentId, file.Name);

            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }
        }
    }
}

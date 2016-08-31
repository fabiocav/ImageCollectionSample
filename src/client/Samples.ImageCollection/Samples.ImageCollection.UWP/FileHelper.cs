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
        public async Task<string> SelectImageAsync(string referenceId)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();

            IStorageFolder baseFolder = await StorageFolder.GetFolderFromPathAsync(GetFilesPath());
            IStorageFolder folder = await baseFolder.CreateFolderAsync(referenceId, CreationCollisionOption.OpenIfExists);

            if (file != null)
            {
                file = await file.CopyAsync(folder, referenceId + Path.GetExtension(file.Name));
                return Path.GetFileName(file.Path);
            }

            return  null;
        }

        public async Task DownloadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file, string targetPath, int attempt = 0)
        {
            try
            {
                await table.DownloadFileAsync(file, targetPath);
            }
            catch (UnauthorizedAccessException)
            {
                DeleteLocalFile(file);

                if (attempt < 3)
                {
                    await Task.Delay(300)
                        .ContinueWith(async t => await DownloadFileAsync(table, file, targetPath, attempt++));
                }
            }
        }

        public async Task UploadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file, string filePath)
        {
            await table.UploadFileAsync(file, filePath);
        }

        public IMobileServiceFileDataSource GetMobileServiceDataSource(string filePath)
        {
            return new PathMobileServiceFileDataSource(filePath);
        }

        private string GetFilesPath()
        {
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "files");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public string GetLocalFilePath(string itemId, string fileName)
        {
            string recordFilesPath = Path.Combine(GetFilesPath(), itemId);

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

        public bool Exists(string filepath) => File.Exists(filepath);
    }
}

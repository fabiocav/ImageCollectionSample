using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace Samples.ImageCollection.Services
{
    public interface IFileHelper
    {
        Task<string> SelectImageAsync(string categoryId);

        string GetLocalFilePath(string itemId, string fileName);

        void DeleteLocalFile(MobileServiceFile file);

        Task UploadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file, string filePath);

        Task DownloadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file, string targetPath, int attempt = 0);

        IMobileServiceFileDataSource GetMobileServiceDataSource(string filePath);
        bool Exists(string filepath);
    }
}

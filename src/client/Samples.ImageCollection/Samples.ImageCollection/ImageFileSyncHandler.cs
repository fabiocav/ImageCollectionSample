using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Files.Metadata;
using Microsoft.WindowsAzure.MobileServices.Files.Sync;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Samples.ImageCollection.Model;
using Samples.ImageCollection.Services;
using Xamarin.Forms;

namespace Samples.ImageCollection
{
    public class ImageFileSyncHandler<T> : IFileSyncHandler where T : class
    {
        private readonly IMobileServiceSyncTable<Category> _categoriesTable;
        private readonly IFileHelper _fileHelper;

        public ImageFileSyncHandler(IMobileServiceSyncTable<Category> categoryTable)
        {
           _categoriesTable = categoryTable;
           _fileHelper = DependencyService.Get<IFileHelper>();
        }

        public Task<IMobileServiceFileDataSource> GetDataSource(MobileServiceFileMetadata metadata)
        {
            var filePath = _fileHelper.GetLocalFilePath(metadata.ParentDataItemId, metadata.FileName);


            IMobileServiceFileDataSource result = null;
            if (filePath != null)
            {
                result = _fileHelper.GetMobileServiceDataSource(filePath);
            }

            return Task.FromResult(result);
        }

        public async Task ProcessFileSynchronizationAction(MobileServiceFile file, FileSynchronizationAction action)
        {
            if (action != FileSynchronizationAction.Delete)
            {
                var filepath = _fileHelper.GetLocalFilePath(file.ParentId, file.Name);
                if (!_fileHelper.Exists(filepath))
                {
                    await _fileHelper.DownloadFileAsync(_categoriesTable, file, filepath);
                }
            }
        }
    }
}

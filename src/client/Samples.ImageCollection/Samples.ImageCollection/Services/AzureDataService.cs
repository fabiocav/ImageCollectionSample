using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Samples.ImageCollection.Model;
using Xamarin.Forms;

namespace Samples.ImageCollection.Services
{
    public class AzureDataService : IDataService
    {
        private IMobileServiceSyncTable<Category> _categoryTable;
        private MobileServiceClient _mobileService = new MobileServiceClient("https://imagecollection.azurewebsites.net/");

        public async Task Initialize()
        {
            var store = new MobileServiceSQLiteStore(Guid.NewGuid().ToString() + "localdata8.db");
            store.DefineTable<Category>();

            _categoryTable = _mobileService.GetSyncTable<Category>();

            this._mobileService.InitializeFileSyncContext(new ImageFileSyncHandler<Category>(_categoryTable), store);
            await _mobileService.SyncContext.InitializeAsync(store, StoreTrackingOptions.AllNotificationsAndChangeDetection);

            //await LoadTestDataAsync();
            await SyncAsync();
        }

        private async Task LoadTestDataAsync()
        {
            await _categoryTable.InsertAsync(new Category { Id = "1", Name = "Bicycles", Description = "Pictures of bicycles" });
            await _categoryTable.InsertAsync(new Category { Id = "2", Name = "Cars", Description = "They go vroom" });
            await _categoryTable.InsertAsync(new Category { Id = "3", Name = "Airplanes", Description = "Look up!" });
            await _categoryTable.InsertAsync(new Category { Id = "4", Name = "Boats", Description = "They float (we hope)" });
        }

        public async Task SyncAsync()
        {
            await _mobileService.SyncContext.PushAsync();
            await _categoryTable.PushFileChangesAsync();

            await _categoryTable.PullAsync("categories", _categoryTable.CreateQuery());
        }

        public Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return _categoryTable.ReadAsync(_categoryTable.CreateQuery());
        }

        public async Task<IEnumerable<ImageReference>> GetImages(string categoryId)
        {
            var fileHelper = DependencyService.Get<IFileHelper>();
            var files = await _categoryTable.GetFilesAsync(new Category { Id = categoryId });

            return files.Select(f => new ImageReference
            {
                Id = f.Id,
                CategoryId = f.ParentId,
                Uri = fileHelper.GetLocalFilePath(categoryId, f.Name)
            });
        }

        public async Task AddImage(ImageReference imageReference)
        {
            string fileName = Path.GetFileName(imageReference.Uri);
            await _categoryTable.AddFileAsync(new Category { Id = imageReference.CategoryId },  fileName);
            await _categoryTable.PushFileChangesAsync();
        }
    }
}

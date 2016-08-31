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
        private MobileServiceClient _mobileService = new MobileServiceClient("https://imagecollectiontest.azurewebsites.net/");
        private IMobileServiceSyncTable<Category> _categoryTable;
        private IMobileServiceSyncTable<ImageReference> _imageReferenceTable;

        public async Task Initialize()
        {
            string storeName = "localdata.db";
            storeName = Guid.NewGuid().ToString() + storeName;

            var store = new MobileServiceSQLiteStore(storeName);
            store.DefineTable<Category>();
            store.DefineTable<ImageReference>();

            _categoryTable = _mobileService.GetSyncTable<Category>();
            _imageReferenceTable = _mobileService.GetSyncTable<ImageReference>();

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
            await _imageReferenceTable.PushFileChangesAsync();

            await _categoryTable.PullAsync("categories", _categoryTable.CreateQuery());
            await _imageReferenceTable.PullAsync("imagerefs", _imageReferenceTable.CreateQuery());
        }

        public Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return _categoryTable.ReadAsync(_categoryTable.CreateQuery());
        }

        public async Task<IEnumerable<ImageReference>> GetImages(string categoryId)
        {
            var fileHelper = DependencyService.Get<IFileHelper>();
            return await _imageReferenceTable.ReadAsync(_imageReferenceTable.CreateQuery().Where(r => r.CategoryId == categoryId));
        }

        public async Task AddImage(ImageReference imageReference)
        {
            string fileName = Path.GetFileName(imageReference.FileName);

            await _imageReferenceTable.InsertAsync(imageReference);
            await _imageReferenceTable.AddFileAsync(imageReference,  fileName);
            await SyncAsync();
        }
    }
}

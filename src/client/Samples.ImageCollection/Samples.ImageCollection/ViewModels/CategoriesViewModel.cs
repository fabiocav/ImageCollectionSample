using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Samples.ImageCollection.Model;
using Samples.ImageCollection.Services;
using Samples.ImageCollection.Views;
using Xamarin.Forms;

namespace Samples.ImageCollection.ViewModels
{
    public class CategoriesViewModel : ViewModel
    {
        private bool _isBusy;
        private ObservableCollection<Category> _categories;
        private IDataService _dataService;
        private readonly INavigation _navigation;
        private readonly IFileHelper _fileHelper;

        public CategoriesViewModel(IDataService dataService, INavigation navigation, IFileHelper fileHelper)
        {
            _dataService = dataService;
            _navigation = navigation;
            _fileHelper = fileHelper;

            CategorySelectedCommand = new Command<Category>(CategorySelected);
            SyncCommand = new Command(async o =>
            {
                await _dataService.SyncAsync();
                await LoadCategories();
            });
        }

        public ICommand CategorySelectedCommand { get; }

        public ICommand SyncCommand { get; }

        public ObservableCollection<Category> Categories
        {
            get
            {
                if (_categories == null)
                {
                    LoadCategories()
                        .ContinueWith(t=>
                        {
                            // log
                        }, TaskContinuationOptions.ExecuteSynchronously);
                }

                return _categories;
            }
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private void CategorySelected(Category category)
        {
            _navigation.PushAsync(new CategoryView(new CategoryViewModel(category, _dataService, _navigation, _fileHelper)));
        }

        private async Task LoadCategories()
        {
            try
            {
                IsBusy = true;

                var categories = await _dataService.GetCategoriesAsync();
                Categories = new ObservableCollection<Category>(categories);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

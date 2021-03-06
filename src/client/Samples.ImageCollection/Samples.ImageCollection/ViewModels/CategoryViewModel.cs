﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Samples.ImageCollection.Model;
using Samples.ImageCollection.Services;
using Xamarin.Forms;

namespace Samples.ImageCollection.ViewModels
{
    public class CategoryViewModel : ViewModel
    {
        private readonly Category _category;
        private readonly IDataService _dataService;
        private ObservableCollection<ImageReferenceViewModel> _images;
        private readonly INavigation _navigation;
        private bool _isBusy;
        private ImageSource _selectedImage;
        private readonly IFileHelper _fileHelper;

        public CategoryViewModel(Category category, IDataService dataService, INavigation navigation, IFileHelper fileHelper)
        {
            _category = category;
            _dataService = dataService;
            _navigation = navigation;
            _fileHelper = fileHelper;

            ShowImageCommand = new Command<ImageReferenceViewModel>(ShowImage);
            AddImageCommand = new Command(async o => await AddImageAsync(o));
        }

        public string CategoryName => _category.Name;

        public ICommand ShowImageCommand { get; }

        public ICommand AddImageCommand { get; }

        public ImageSource SelectedImage
        {
            get
            {
                return _selectedImage;
            }
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<ImageReferenceViewModel> Images
        {
            get
            {
                if (_images == null)
                {
                    LoadImagesAsync()
                        .ContinueWith(t =>
                        {
                            // log
                        }, TaskContinuationOptions.ExecuteSynchronously);
                }

                return _images;
            }
            set
            {
                _images = value;
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

        private async Task LoadImagesAsync()
        {
            try
            {
                IsBusy = true;

                var categories = await _dataService.GetImages(_category.Id);
                Images = new ObservableCollection<ImageReferenceViewModel>(categories.Select(i=>new ViewModels.ImageReferenceViewModel(i, _fileHelper)));

                if (Images.Any())
                {
                    SelectedImage = Images.First().ImageSource;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ShowImage(ImageReferenceViewModel obj)
        {
            SelectedImage = obj.ImageSource;
        }

        private async Task AddImageAsync(object obj)
        {
            string fileId = Guid.NewGuid().ToString();
            var photo = await _fileHelper.SelectImageAsync(fileId);

            if (photo != null)
            {
                var image = new ImageReference
                {
                    Id = fileId,
                    CategoryId = _category.Id,
                    FileName = photo
                };

                await _dataService.AddImage(image);
                Images.Add(new ImageReferenceViewModel(image, _fileHelper));
            }
        }
    }
}

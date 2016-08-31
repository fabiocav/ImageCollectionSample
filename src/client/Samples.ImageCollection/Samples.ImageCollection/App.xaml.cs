using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samples.ImageCollection.Services;
using Samples.ImageCollection.ViewModels;
using Samples.ImageCollection.Views;
using Xamarin.Forms;

namespace Samples.ImageCollection
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SplashView();

            var dataService = new AzureDataService();

            dataService.Initialize()
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Debug.WriteLine("Faulted");
                    }

                    var categoriesPage = new CategoriesView();
                    NavigationPage root = new NavigationPage(categoriesPage);

                    IFileHelper fileHelper = DependencyService.Get<IFileHelper>();
                    categoriesPage.ViewModel = new CategoriesViewModel(dataService, root.Navigation, fileHelper);
                    MainPage = root;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}

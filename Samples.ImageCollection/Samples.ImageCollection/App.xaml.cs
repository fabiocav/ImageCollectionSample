using System;
using System.Collections.Generic;
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

            var categoriesPage = new CategoriesView();
            NavigationPage root = new NavigationPage(categoriesPage);
            categoriesPage.ViewModel = new CategoriesViewModel(new MockDataService(), root.Navigation);
            MainPage = root;
        }
    }
}

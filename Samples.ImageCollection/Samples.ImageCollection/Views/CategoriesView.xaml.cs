using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samples.ImageCollection.Model;
using Samples.ImageCollection.ViewModels;
using Xamarin.Forms;

namespace Samples.ImageCollection.Views
{
    public partial class CategoriesView : ContentPage
    {
        public CategoriesView()
        {
            InitializeComponent();
        }

        public CategoriesViewModel ViewModel
        {
            get { return BindingContext as CategoriesViewModel; }
            set { BindingContext = value; }
        }
    }
}

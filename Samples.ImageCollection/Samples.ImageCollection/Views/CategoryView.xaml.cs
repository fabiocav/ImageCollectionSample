using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samples.ImageCollection.ViewModels;
using Xamarin.Forms;

namespace Samples.ImageCollection.Views
{
    public partial class CategoryView : ContentPage
    {
        public CategoryView(CategoryViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        public CategoryViewModel ViewModel
        {
            get { return BindingContext as CategoryViewModel; }
            set { BindingContext = value; }
        }
    }
}

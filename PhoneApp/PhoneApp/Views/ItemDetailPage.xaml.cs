using PhoneApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PhoneApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
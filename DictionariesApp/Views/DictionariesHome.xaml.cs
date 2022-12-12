using DictionariesApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DictionariesApp.Views
{
    public partial class DictionariesHome : Page
    {
        public DictionariesHome()
        {
            this.DataContext = new HomePageViewModel();
            InitializeComponent();
        }

        private void ClickDeleteDictionary(object sender, RoutedEventArgs e)
        {
            (this.DataContext as HomePageViewModel).SwitchIsDeleting();
        }

        private void ClickDictionaryName(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as HomePageViewModel;
            var b = sender as Button;
            if (vm.IsDeleting)
            {
                vm.DeleteDictionary(b.Content as string);
                return;
            }
            var translationPage = new DictionariesTranslationPage(b.Content as string);
            this.NavigationService.Navigate(translationPage);
        }

        private void ClickAddDictionary(object sender, RoutedEventArgs e)
        {
            PopupAddDictionary.IsOpen = true;
        }

        private void ClickCloseInPopup(object sender, RoutedEventArgs e)
        {
            PopupAddDictionary.IsOpen = false;
            (this.DataContext as HomePageViewModel).ClearTextBoxes();
        }

        private void ClickAddInPopup(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as HomePageViewModel;
            if (vm.AddDictionary())
            {
                PopupAddDictionary.IsOpen = false;
                (this.DataContext as HomePageViewModel).ClearTextBoxes();
            }
        }
    }
}

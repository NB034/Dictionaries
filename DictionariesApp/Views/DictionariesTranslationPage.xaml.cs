using DictionariesApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DictionariesApp.Views
{
    public partial class DictionariesTranslationPage : Page
    {
        public DictionariesTranslationPage()
        {
            InitializeComponent();
        }

        public DictionariesTranslationPage(string title) : this()
        {
            this.DataContext = new TranslationPageViewModel(title);
        }

        private void ClickSearch(object sender, RoutedEventArgs e)
        {
            (this.DataContext as TranslationPageViewModel).SearchTranslations();
        }

        private void ClickAddWord(object sender, RoutedEventArgs e)
        {
            PopupAddWord.IsOpen = true;
        }

        private void ClickAddInAddWordPopup(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as TranslationPageViewModel;
            if (vm.AddWord())
            {
                PopupAddWord.IsOpen = false;
                vm.ClearTextBoxes();
            }
        }

        private void ClickCloseInAddWordPopup(object sender, RoutedEventArgs e)
        {
            PopupAddWord.IsOpen = false;
            (this.DataContext as TranslationPageViewModel).ClearTextBoxes();
        }

        private void ClickDeleteWord(object sender, RoutedEventArgs e)
        {
            (this.DataContext as TranslationPageViewModel).DeleteWord();
        }

        private void ClickAddTranslation(object sender, RoutedEventArgs e)
        {
            PopupAddTranslation.IsOpen = true;
        }

        private void ClickAddInAddTranslationPopup(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as TranslationPageViewModel;
            if (vm.AddTranslation())
            {
                PopupAddTranslation.IsOpen = false;
                vm.ClearTextBoxes();
            }
        }

        private void ClickCloseInAddTranslationPopup(object sender, RoutedEventArgs e)
        {
            PopupAddTranslation.IsOpen = false;
            (this.DataContext as TranslationPageViewModel).ClearTextBoxes();
        }

        private void ClickDeleteTranslation(object sender, RoutedEventArgs e)
        {
            (this.DataContext as TranslationPageViewModel).SwitchIsDeleting();
        }

        private void ClickTranslation(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as TranslationPageViewModel;
            if (vm.IsDeleting)
            {
                var b = sender as Button;
                vm.DeleteTranslation(b.Content as string);
            }
        }
    }
}
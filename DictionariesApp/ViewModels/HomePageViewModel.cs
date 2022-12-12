using DictionariesApp.Exceptions;
using DictionariesApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DictionariesApp.ViewModels
{
    internal class HomePageViewModel : BaseViewModel
    {
        private HomePageModel model;
        public ObservableCollection<string> Titles { get; set; }

        public string FirstLanguageInPopup { get; set; }
        public string SecondLanguageInPopup { get; set; }

        public bool IsDeleting { get; set; }
        public string Status => IsDeleting ? "Deletion enabled" : "";

        public HomePageViewModel()
        {
            try
            {
                model = new HomePageModel();
                Titles = new ObservableCollection<string>();
                RefreshTitles();
                IsDeleting = false;
            }
            catch (InvalidDbDataException ex)
            {
                ShowMessageBox(ex.Message);
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        public void ClearTextBoxes()
        {
            FirstLanguageInPopup = "";
            OnPropertyChanged(nameof(FirstLanguageInPopup));

            SecondLanguageInPopup = "";
            OnPropertyChanged(nameof(SecondLanguageInPopup));
        }

        public bool AddDictionary()
        {
            if (!IsLanguageValid(FirstLanguageInPopup))
            {
                ShowMessageBox("The invalid name of the first language");
                return false;
            }
            if (!IsLanguageValid(SecondLanguageInPopup))
            {
                ShowMessageBox("The invalid name of the second language");
                return false;
            }
            try
            {
                model.AddDictionary(FirstLanguageInPopup, SecondLanguageInPopup);
                RefreshTitles();
                return true;
            }
            catch (InvalidDbDataException ex)
            {
                ShowMessageBox(ex.Message);
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
            return false;
        }

        public void DeleteDictionary(string title)
        {
            try
            {
                string[] languages = title.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                model.DeleteDictionary(languages[0], languages[1]);
                RefreshTitles();
            }
            catch (InvalidDbDataException ex)
            {
                ShowMessageBox(ex.Message);
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        public void SwitchIsDeleting()
        {
            IsDeleting = !IsDeleting;
            OnPropertyChanged(nameof(Status));
        }

        private void RefreshTitles()
        {
            Titles.Clear();
            foreach (var tuple in model.GetDictionaries())
            {
                Titles.Add($"{tuple.Item1} - {tuple.Item2}");
            }
            OnPropertyChanged(nameof(Titles));
        }

        private void ShowMessageBox(string message) =>
            MessageBox.Show(message, "Exception", MessageBoxButton.OK);

        private bool IsLanguageValid(string language)
        {
            if(language == null) return false;
            if(language.Length == 0) return false;
            if (language.Any(Char.IsDigit)) return false;
            if (language.Contains(" - ")) return false;
            return true;
        }
    }
}

using DictionariesApp.Exceptions;
using DictionariesApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace DictionariesApp.ViewModels
{
    internal class TranslationPageViewModel : BaseViewModel
    {
        private TranslationPageModel model;
        public string Title { get; set; }
        public ObservableCollection<string> Translations { get; set; }

        public string KeyWord { get; set; }
        public string WordInAddWordPopup { get; set; }
        public string TranslationInAddWordPopup { get; set; }
        public string TranslationInAddTranslationPopup { get; set; }

        public bool IsDeleting { get; set; }
        public string Status => IsDeleting ? "Deletion enabled" : "";

        public TranslationPageViewModel(string title)
        {
            try
            {
                Translations = new ObservableCollection<string>();
                Title = title;
                string[] languages = Title.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                model = new TranslationPageModel(languages[0], languages[1]);
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
            WordInAddWordPopup = "";
            OnPropertyChanged(nameof(WordInAddWordPopup));

            TranslationInAddWordPopup = "";
            OnPropertyChanged(nameof(TranslationInAddWordPopup));

            TranslationInAddTranslationPopup = "";
            OnPropertyChanged(nameof(TranslationInAddTranslationPopup));
        }

        public void SearchTranslations()
        {
            if (!IsWordValid(KeyWord))
            {
                ShowMessageBox("Search word is not valid");
                return;
            }
            try
            {
                RefreshTranslations();
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

        public bool AddWord()
        {
            if (!IsWordValid(WordInAddWordPopup))
            {
                ShowMessageBox("Word is not valid");
                return false;
            }
            if (!IsWordValid(TranslationInAddWordPopup))
            {
                ShowMessageBox("Translation is not valid");
                return false;
            }
            try
            {
                model.AddWord(WordInAddWordPopup, TranslationInAddWordPopup);
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

        public void DeleteWord()
        {
            try
            {
                model.DeleteWord(KeyWord);
                Translations.Clear();
                OnPropertyChanged(nameof(Translations));
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

        public bool AddTranslation()
        {
            if (!IsWordValid(KeyWord))
            {
                ShowMessageBox("Word is not valid");
                return false;
            }
            if (!IsWordValid(TranslationInAddTranslationPopup))
            {
                ShowMessageBox("Translation is not valid");
                return false;
            }
            try
            {
                model.AddTranslation(KeyWord, TranslationInAddTranslationPopup);
                RefreshTranslations();
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

        public void DeleteTranslation(string translation)
        {
            if (Translations.Count == 1)
            {
                ShowMessageBox("The last translation cannot be deleted");
                return;
            }
            try
            {
                model.DeleteTranslation(KeyWord, translation);
                RefreshTranslations();
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

        private void RefreshTranslations()
        {
            Translations = new ObservableCollection<string>(model.GetTranslations(KeyWord));
            OnPropertyChanged(nameof(Translations));
        }

        private void ShowMessageBox(string message) =>
            MessageBox.Show(message, "Exception", MessageBoxButton.OK);

        private bool IsWordValid(string word)
        {
            if (word == null) return false;
            if (word.Length == 0) return false;
            if (word.Any(Char.IsDigit)) return false;
            return true;
        }
    }
}

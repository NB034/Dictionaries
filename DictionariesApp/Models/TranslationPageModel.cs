using DictionariesApp.DataAccess.Entities;
using DictionariesApp.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace DictionariesApp.Models
{
    public class TranslationPageModel : BaseModel
    {
        public LanguageEntity MainLanguage { get; set; }
        public LanguageEntity SecondaryLanguage { get; set; }

        public TranslationPageModel(string mainLanguage, string secondaryLanguage)
        {
            MainLanguage = GetLanguages().Select(lan => lan).Where(lan => lan.Name == mainLanguage).First();
            SecondaryLanguage = GetLanguages().Select(lan => lan).Where(lan => lan.Name == secondaryLanguage).First();
            if (MainLanguage == null || SecondaryLanguage == null)
            {
                throw new InvalidDbDataException("The language does not exist");
            }
        }

        public List<string> GetTranslations(string searchWord)
        {
            searchWord = searchWord.ToLowerInvariant();
            WordEntity word = GetWords().Select(w => w)
                .Where(w => w.Name == searchWord && w.LanguageId == MainLanguage.Id).FirstOrDefault();
            if (word == null)
            {
                throw new InvalidDbDataException("Word does not exist");
            }
            List<string> translations = GetTranslationsEntitiesOf(word).Select(w => w.Name).ToList();
            if (translations.Count == 0)
            {
                throw new InvalidDbDataException("The translations were not found");
            }
            return translations;
        }

        public void AddWord(string word, string translation)
        {
            if (GetWords().Select(w => w).Where(w => w.Name == word && w.LanguageId == MainLanguage.Id).Any())
            {
                throw new InvalidDbDataException("The word is already exist");
            }
            AddWordEntityIfNotExist(MainLanguage.Id, word);
            AddWordEntityIfNotExist(SecondaryLanguage.Id, translation);
            WordEntity first = GetWords().Select(w => w)
                .Where(w => w.LanguageId == MainLanguage.Id && w.Name == word).First();
            WordEntity second = GetWords().Select(w => w)
                .Where(w => w.LanguageId == SecondaryLanguage.Id && w.Name == translation).First();
            AddWordPair(first, second);
        }

        public void DeleteWord(string word)
        {
            if (!GetWords().Select(w => w).Where(w => w.Name == word && w.LanguageId == MainLanguage.Id).Any())
            {
                throw new InvalidDbDataException("The word does not exist");
            }
            WordEntity wordEntity = GetWords().Select(w => w)
                .Where(w => w.LanguageId == MainLanguage.Id && w.Name == word).First();
            DeleteAllTranslationsOf(wordEntity);
            DeleteWordIfNotUsed(wordEntity);
        }

        public void AddTranslation(string word, string translation)
        {
            if (!GetWords().Select(w => w).Where(w => w.Name == word).Any())
            {
                throw new InvalidDbDataException("The word is not exist");
            }
            if (GetWords().Select(w => w).Where(w => w.Name == translation).Any())
            {
                throw new InvalidDbDataException("The translation is already exist");
            }
            AddWordEntityIfNotExist(SecondaryLanguage.Id, translation);
            WordEntity first = GetWords().Select(w => w)
                .Where(w => w.LanguageId == MainLanguage.Id && w.Name == word).First();
            WordEntity second = GetWords().Select(w => w)
                .Where(w => w.LanguageId == SecondaryLanguage.Id && w.Name == translation).First();
            AddWordPair(first, second);
        }

        public void DeleteTranslation(string word, string translation)
        {
            if (!GetWords().Select(w => w).Where(w => w.Name == word).Any())
            {
                throw new InvalidDbDataException("The word is not exist");
            }
            if (!GetWords().Select(w => w).Where(w => w.Name == translation).Any())
            {
                throw new InvalidDbDataException("The translation is not exist");
            }
            WordEntity wordEntity = GetWords().Select(w => w)
                .Where(w => w.LanguageId == MainLanguage.Id && w.Name == word).First();
            WordEntity translationEntity = GetWords().Select(w => w)
                .Where(w => w.LanguageId == SecondaryLanguage.Id && w.Name == translation).First();
            DeletePair(wordEntity, translationEntity);
            DeleteWordIfNotUsed(translationEntity);
        }

        private void DeleteWordIfNotUsed(WordEntity word)
        {
            var repo = MakeConnection();
            if (!repo.GetWordPairs().Any(p => p.FirstWordId == word.Id || p.SecondWordId == word.Id))
            {
                repo.DeleteWord(word.Id);
                repo.SaveChanges();
            }
        }

        private void DeletePair(WordEntity word, WordEntity translation)
        {
            var repo = MakeConnection();
            var selection = from pair in repo.GetWordPairs()
                            where (pair.FirstWordId == word.Id && pair.SecondWordId == translation.Id)
                            || (pair.SecondWordId == word.Id && pair.FirstWordId == translation.Id)
                            select pair;
            foreach(var pair in selection)
            {
                repo.DeleteWordPair(pair.Id);
            }
            repo.SaveChanges();
        }

        private void DeleteAllTranslationsOf(WordEntity wordEntity)
        {
            var repo = MakeConnection();
            var transalations = GetTranslationsEntitiesOf(wordEntity);
            DeleteWordPairsThatContains(wordEntity);
            foreach(var word in transalations)
            {
                repo.DeleteWord(word.Id);
            }
            repo.SaveChanges();
        }

        private void DeleteWordPairsThatContains(WordEntity wordEntity)
        {
            var repo = MakeConnection();
            var selection = from pair in repo.GetWordPairs()
                            where pair.FirstWordId == wordEntity.Id || pair.SecondWordId == wordEntity.Id
                            select pair;
            foreach(var pair in selection)
            {
                repo.DeleteWordPair(pair.Id);
            }
            repo.SaveChanges();
        }

        private WordEntity[] GetTranslationsEntitiesOf(WordEntity word)
        {
            var repo = MakeConnection();
            var translations = (from translation in repo.GetWords()
                                join pair in repo.GetWordPairs() on translation.Id equals pair.SecondWordId
                                where pair.FirstWordId == word.Id
                                && translation.LanguageId == SecondaryLanguage.Id
                                select translation)
                                .Union
                                (from translation in repo.GetWords()
                                 join pair in repo.GetWordPairs() on translation.Id equals pair.FirstWordId
                                 where pair.SecondWordId == word.Id
                                 && translation.LanguageId == SecondaryLanguage.Id
                                 select translation);
            return translations.ToArray();
        }

        private void AddWordEntityIfNotExist(int languageId, string word)
        {
            var repo = MakeConnection();
            if (!repo.GetWords().Select(w => w).Where(w => w.Name == word && w.LanguageId == languageId).Any())
            {
                repo.AddWord(new WordEntity(languageId, word));
            }
            repo.SaveChanges();
        }

        private void AddWordPair(WordEntity fisrtWord, WordEntity secondWord)
        {
            var repo = MakeConnection();
            repo.AddWordPair(new WordPairEntity(fisrtWord.Id, secondWord.Id));
            repo.SaveChanges();
        }
    }
}

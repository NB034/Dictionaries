using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DictionariesApp.DataAccess.Entities;
using DictionariesApp.DataAccess.Repositories;
using DictionariesApp.Exceptions;

namespace DictionariesApp.Models
{
    public class HomePageModel : BaseModel
    {
        public List<Tuple<string, string>> GetDictionaries()
        {
            var dictionaries = new List<Tuple<string, string>>();
            foreach (var pair in GetLanguagePairs())
            {
                var tuple = GetLanguagesFromLanguagePair(pair);
                dictionaries.Add(tuple);
                dictionaries.Add(Tuple.Create(tuple.Item2, tuple.Item1));
            }
            return dictionaries;
        }

        public void AddDictionary(string firstLan, string secondLan)
        {
            if (firstLan.Length == 0 || secondLan.Length == 0)
            {
                throw new InvalidDbDataException("Language name cannot be empty");
            }
            CorrectFormat(ref firstLan);
            CorrectFormat(ref secondLan);
            AddLanguageIfNotExist(firstLan);
            AddLanguageIfNotExist(secondLan);
            AddLanguagePairIfNotExist(firstLan, secondLan);
        }

        public void DeleteDictionary(string firstLan, string secondLan)
        {
            if (!(GetLanguages().Select(lan => lan.Name).Contains(firstLan))
                && GetLanguages().Select(lan => lan.Name).Contains(secondLan))
            {
                throw new InvalidDbDataException("Language does not exist");
            }
            int firstId = GetLanguageId(firstLan);
            int secondId = GetLanguageId(secondLan);
            if (!IsLanguagePairExist(firstId, secondId))
            {
                throw new InvalidDbDataException("Language pair does not exist");
            }
            DeleteWordPairs(firstId, secondId);
            DeleteUnusedWords();
            DeleteLanguagePair(firstId, secondId);
            DeleteUnusedLanguages();
        }

        private void DeleteUnusedLanguages()
        {
            var repo = MakeConnection();
            var selection = from lan in repo.GetLanguages()
                            where !repo.GetLanguagePairs().Select(p => p).Where(p => p.FirstLanguageId == lan.Id).Any()
                            && !repo.GetLanguagePairs().Select(p => p).Where(p => p.SecondLanguageId == lan.Id).Any()
                            && !repo.GetWords().Select(w => w).Where(w => w.LanguageId == lan.Id).Any()
                            select lan;
            foreach (var lan in selection)
            {
                repo.DeleteLanguage(lan.Id);
            }
            repo.SaveChanges();
        }

        private void DeleteLanguagePair(int firstLanId, int secondLanId)
        {
            var repo = MakeConnection();
            var selection = from pair in repo.GetLanguagePairs()
                            where (pair.FirstLanguageId == firstLanId && pair.SecondLanguageId == secondLanId)
                            || (pair.FirstLanguageId == secondLanId && pair.SecondLanguageId == firstLanId)
                            select pair;
            foreach (var pair in selection)
            {
                repo.DeleteLanguagePair(pair.Id);
            }
            repo.SaveChanges();
        }

        private void DeleteUnusedWords()
        {
            var repo = MakeConnection();
            var selection = from word in repo.GetWords()
                            where !repo.GetWordPairs().Select(p => p).Where(p => p.FirstWordId == word.Id).Any()
                            && !repo.GetWordPairs().Select(p => p).Where(p => p.SecondWordId == word.Id).Any()
                            select word;
            foreach (var word in selection)
            {
                repo.DeleteWord(word.Id);
            }
            repo.SaveChanges();
        }

        private void DeleteWordPairs(int firstLanId, int secondLanId)
        {
            var repo = MakeConnection();
            var selection = from pair in repo.GetWordPairs()
                            join firstWord in repo.GetWords() on pair.FirstWordId equals firstWord.Id
                            join secondWord in repo.GetWords() on pair.SecondWordId equals secondWord.Id
                            where (firstWord.LanguageId == firstLanId && secondWord.LanguageId == secondLanId)
                                     || (firstWord.LanguageId == secondLanId && secondWord.Id == firstLanId)
                            select pair;
            foreach (var pair in selection)
            {
                repo.DeleteWordPair(pair.Id);
            }
            repo.SaveChanges();
        }

        private Tuple<string, string> GetLanguagesFromLanguagePair(LanguagePairEntity pair)
        {
            string first = (from lan in GetLanguages()
                            where pair.FirstLanguageId == lan.Id
                            select lan.Name).FirstOrDefault();
            string second = (from lan in GetLanguages()
                             where pair.SecondLanguageId == lan.Id
                             select lan.Name).FirstOrDefault();
            return new Tuple<string, string>(first, second);
        }

        private void AddLanguagePairIfNotExist(string firstLan, string secondLan)
        {
            int firstId = GetLanguageId(firstLan);
            int secondId = GetLanguageId(secondLan);
            if (!IsLanguagePairExist(firstId, secondId))
            {
                TranslationsRepository repository = MakeConnection();
                repository.AddLanguagePair(new LanguagePairEntity(firstId, secondId));
                repository.SaveChanges();
            }
        }

        private void AddLanguageIfNotExist(string language)
        {
            if (!GetLanguages().Select(lan => lan.Name).Contains(language))
            {
                TranslationsRepository repository = MakeConnection();
                repository.AddLanguage(new LanguageEntity(language));
                repository.SaveChanges();
            }
        }

        private int GetLanguageId(string name)
        {
            return (from lan in GetLanguages()
                    where lan.Name == name
                    select lan.Id).First();
        }

        private bool IsLanguagePairExist(int firstId, int secondId)
        {
            return GetLanguagePairs().Select(pair => pair)
                .Where(pair => (pair.FirstLanguageId == firstId && pair.SecondLanguageId == secondId)
                || (pair.FirstLanguageId == secondId && pair.SecondLanguageId == firstId)).Any();
        }

        private void CorrectFormat(ref string str)
        {
            StringBuilder builder = new StringBuilder(str.ToLowerInvariant());
            builder[0] = Char.ToUpperInvariant(builder[0]);
            str = builder.ToString();
        }
    }
}

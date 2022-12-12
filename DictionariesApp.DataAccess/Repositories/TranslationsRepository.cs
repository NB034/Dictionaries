using DictionariesApp.DataAccess.Contexts;
using DictionariesApp.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DictionariesApp.DataAccess.Repositories
{
    public class TranslationsRepository : ITranslationsRepository
    {
        private readonly DictionariesDbContext _dictionariesDbContext;

        public TranslationsRepository(DictionariesDbContext dictionariesDbContext)
        {
            this._dictionariesDbContext = dictionariesDbContext;
        }

        public void SaveChanges() => 
            _dictionariesDbContext.SaveChanges();

        public LanguageEntity[] GetLanguages() => 
            _dictionariesDbContext.Languages.ToArray<LanguageEntity>();


        public void AddLanguage(LanguageEntity language) =>
            _dictionariesDbContext.Languages.Add(language);

        public void DeleteLanguage(int id)
        {
            IEnumerable<LanguageEntity> entities = _dictionariesDbContext.Languages.Where(e => e.Id == id);
            _dictionariesDbContext.Languages.RemoveRange(entities);
        }

        public LanguagePairEntity[] GetLanguagePairs() => 
            _dictionariesDbContext.LanguagePairs.ToArray();

        public void AddLanguagePair(LanguagePairEntity pair) =>
            _dictionariesDbContext.LanguagePairs.Add(pair);

        public void DeleteLanguagePair(int id)
        {
            IEnumerable<LanguagePairEntity> entities = _dictionariesDbContext.LanguagePairs.Where(e => e.Id == id);
            _dictionariesDbContext.LanguagePairs.RemoveRange(entities);
        }

        public WordEntity[] GetWords() =>
            _dictionariesDbContext.Words.ToArray<WordEntity>();

        public void AddWord(WordEntity word) => 
            _dictionariesDbContext.Words.Add(word);

        public void DeleteWord(int id)
        {
            IEnumerable<WordEntity> entities = _dictionariesDbContext.Words.Where(e => e.Id == id);
            _dictionariesDbContext.Words.RemoveRange(entities);
        }

        public WordPairEntity[] GetWordPairs() => 
            _dictionariesDbContext.WordPairs.ToArray<WordPairEntity>();

        public void AddWordPair(WordPairEntity pair) => 
            _dictionariesDbContext.WordPairs.Add(pair);

        public void DeleteWordPair(int id)
        {
            IEnumerable<WordPairEntity> entities = _dictionariesDbContext.WordPairs.Where(e => e.Id == id);
            _dictionariesDbContext.WordPairs.RemoveRange(entities);
        }
    }
}

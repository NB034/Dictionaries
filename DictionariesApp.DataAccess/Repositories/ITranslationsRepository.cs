using DictionariesApp.DataAccess.Entities;

namespace DictionariesApp.DataAccess.Repositories
{
    public interface ITranslationsRepository
    {
        void SaveChanges();

        LanguageEntity[] GetLanguages();
        void AddLanguage(LanguageEntity language);
        void DeleteLanguage(int id);

        LanguagePairEntity[] GetLanguagePairs();
        void AddLanguagePair(LanguagePairEntity pair);
        void DeleteLanguagePair(int id);

        WordEntity[] GetWords();
        void AddWord(WordEntity word);
        void DeleteWord(int id);

        WordPairEntity[] GetWordPairs();
        void AddWordPair(WordPairEntity pair);
        void DeleteWordPair(int id);
    }
}

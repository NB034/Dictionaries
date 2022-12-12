using DictionariesApp.DataAccess.Contexts;
using DictionariesApp.DataAccess.Entities;
using DictionariesApp.DataAccess.Repositories;
using System.Configuration;

namespace DictionariesApp.Models
{
    public class BaseModel
    {
        protected string _connectionString;

        public BaseModel() =>
            _connectionString = ConfigurationManager.ConnectionStrings["Defaultconnection"].ConnectionString;

        protected DictionariesDbContext CreateContext() =>
            new DictionariesDbContext(_connectionString);

        protected TranslationsRepository MakeConnection() =>
            new TranslationsRepository(CreateContext());

        public LanguageEntity[] GetLanguages() => 
            MakeConnection().GetLanguages();

        public LanguagePairEntity[] GetLanguagePairs() => 
            MakeConnection().GetLanguagePairs();

        public WordEntity[] GetWords() => 
            MakeConnection().GetWords();

        public WordPairEntity[] GetWordPairs() => 
            MakeConnection().GetWordPairs();
    }
}

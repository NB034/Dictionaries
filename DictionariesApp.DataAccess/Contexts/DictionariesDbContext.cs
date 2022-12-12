using DictionariesApp.DataAccess.Entities;
using System.Data.Entity;

namespace DictionariesApp.DataAccess.Contexts
{
    public class DictionariesDbContext : DbContext
    {
        public DbSet<LanguageEntity> Languages { get; set; }
        public DbSet<LanguagePairEntity> LanguagePairs { get; set; }
        public DbSet<WordEntity> Words { get; set; }
        public DbSet<WordPairEntity> WordPairs { get; set; }

        public DictionariesDbContext(string connectionString) : base(connectionString) { }
    }
}
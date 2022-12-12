using System.ComponentModel.DataAnnotations.Schema;

namespace DictionariesApp.DataAccess.Entities
{
    [Table(name: "Words")]
    public class WordEntity
    {
        [Column("id")]
        public int Id { get; private set; }

        [Column("language_id")]
        public int LanguageId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public WordEntity()
        {
        }

        public WordEntity(int languageId, string name)
        {
            LanguageId = languageId;
            Name = name;
        }
    }
}

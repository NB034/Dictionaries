using System.ComponentModel.DataAnnotations.Schema;

namespace DictionariesApp.DataAccess.Entities
{
    [Table(name: "Languages")]
    public class LanguageEntity
    {
        [Column("id")]
        public int Id { get; private set; }

        [Column("name")]
        public string Name { get; set; }

        public LanguageEntity()
        {
        }

        public LanguageEntity(string name)
        {
            Name = name;
        }
    }
}
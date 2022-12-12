using System.ComponentModel.DataAnnotations.Schema;

namespace DictionariesApp.DataAccess.Entities
{
    [Table(name: "LanguagePairs")]
    public class LanguagePairEntity
    {
        [Column("id")]
        public int Id { get; private set; }

        [Column("first_language_id")]
        public int FirstLanguageId { get; set; }

        [Column("second_language_id")]
        public int SecondLanguageId { get; set; }

        public LanguagePairEntity()
        {
        }

        public LanguagePairEntity(int firstLanguageId, int secondLanguageId)
        {
            FirstLanguageId = firstLanguageId;
            SecondLanguageId = secondLanguageId;
        }
    }
}

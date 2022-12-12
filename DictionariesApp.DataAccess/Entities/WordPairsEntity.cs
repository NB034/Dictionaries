using System.ComponentModel.DataAnnotations.Schema;

namespace DictionariesApp.DataAccess.Entities
{
    [Table(name: "WordPairs")]
    public class WordPairEntity
    {
        [Column("id")]
        public int Id { get; private set; }

        [Column("first_word_id")]
        public int FirstWordId { get; set; }

        [Column("second_word_id")]
        public int SecondWordId { get; set; }

        public WordPairEntity()
        {
        }

        public WordPairEntity(int firstWordId, int secondWordId)
        {
            FirstWordId= firstWordId;
            SecondWordId= secondWordId;
        }
    }
}

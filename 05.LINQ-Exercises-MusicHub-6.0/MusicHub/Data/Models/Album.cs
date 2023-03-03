using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            this.Songs = new List<Song>();
        }
        public int Id { get; set; }

        [StringLength(40)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public decimal Price { get { return this.Songs.Sum(s => s.Price); } }

        [ForeignKey(nameof(Producer))]
        public int? ProducerId { get; set; }

        public Producer? Producer { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}
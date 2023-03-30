using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class Coach
    {
        public Coach()
        {
            this.Footballers = new List<Footballer>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        public string Nationality { get; set; }

        public ICollection<Footballer> Footballers { get; set; }
    }
}

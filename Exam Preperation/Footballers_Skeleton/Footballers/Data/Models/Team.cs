using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class Team
    {
        public Team()
        {
            this.TeamsFootballers = new List<TeamFootballer>();    
        }

        [Key]
        public int Id { get; set; }

        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"/[A-Z, a-z, ., ,-]+")]
        public string Name { get; set; }

        [StringLength(40, MinimumLength = 2)]
        public string Nationality { get; set; }

        public int Trophies { get; set; }

        public ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}

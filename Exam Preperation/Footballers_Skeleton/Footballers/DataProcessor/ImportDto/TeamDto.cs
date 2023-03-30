using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.DataProcessor.ImportDto
{
    public class TeamDto
    {
        public TeamDto()
        {
            this.Footballers = new HashSet<int>();
        }

        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-]+$")]
        public string Name { get; set; }

        [StringLength(40, MinimumLength = 2)]
        public string Nationality { get; set; }

        public int Trophies { get; set; }

        public HashSet<int> Footballers { get; set; }
    }
}

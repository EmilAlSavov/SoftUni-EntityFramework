using Footballers.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.DataProcessor.ImportDto
{
    public class CoachDto
    {
        public CoachDto()
        {
            this.Footballers = new List<FootballerDto>();
        }

        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        public string Nationality { get; set; }

        public ICollection<FootballerDto> Footballers { get; set; }
    }
}

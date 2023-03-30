using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class CoachExportDto
    {
        public CoachExportDto()
        {
            this.Footballers = new List<FootballerCoachDto>();
        }
        public string CoachName { get; set; }

        [XmlAttribute]
        public int FootballersCount { get; set; }

        public List<FootballerCoachDto> Footballers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class DespatcherExportDto
    {
        public string DespatcherName { get; set; }

        [XmlAttribute]
        public int TrucksCount { get; set; }

        public List<TruckExportDto> Trucks { get; set; }
    }
}

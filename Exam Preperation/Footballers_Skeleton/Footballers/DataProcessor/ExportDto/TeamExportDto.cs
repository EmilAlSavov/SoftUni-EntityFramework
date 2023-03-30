using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.DataProcessor.ExportDto
{
    public class TeamExportDto
    {
        public string Name { get; set; }

        public List<FootballerExportDto> Footballers { get; set; }
    }
}

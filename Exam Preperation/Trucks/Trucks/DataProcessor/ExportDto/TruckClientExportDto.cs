using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.DataProcessor.ExportDto
{
    public class TruckClientExportDto
    {
        public string TruckRegistrationNumber { get; set; }

        public string VinNumber { get; set; }

        public int TankCapacity { get; set; }

        public int CargoCapacity { get; set; }

        public string CategoryType { get; set; }

        public string MakeType { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.DataProcessor.ExportDto
{
    public class ClientExportDto
    {
        public string Name { get; set; }

        public List<TruckClientExportDto> Trucks { get; set; }
    }
}

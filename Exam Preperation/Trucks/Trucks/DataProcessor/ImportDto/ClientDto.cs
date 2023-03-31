using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    public class ClientDto
    {
        public ClientDto()
        {
            this.Trucks = new HashSet<int>();
        }
        public string Name { get; set; }

        public string Nationality { get; set; }

        public string Type { get; set; }

        public HashSet<int> Trucks { get; set; }
    }
}

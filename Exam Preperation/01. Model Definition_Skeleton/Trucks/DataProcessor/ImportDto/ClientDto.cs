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
            this.Trucks = new List<ClientTruck>();
        }
        public string Name { get; set; }

        public string Nationality { get; set; }

        public string Type { get; set; }

        public List<ClientTruck> Trucks { get; set; }
    }
}

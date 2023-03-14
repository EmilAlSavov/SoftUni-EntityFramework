using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Export
{
    public class ProductExportDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        [JsonIgnore]
        public string? Seller { get; set; }

        public string? BuyerFirstName { get; set; }

        public string? BuyerLastName { get; set; }
    }
}

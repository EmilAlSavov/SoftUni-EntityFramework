using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Export
{
    public class UserSoldProductsDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<ProductExportDto> Products { get; set; }
    }
}

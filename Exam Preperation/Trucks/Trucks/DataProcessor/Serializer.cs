namespace Trucks.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.IdentityModel.Tokens.Jwt;
    using System.Xml.Serialization;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatchers = context.Despatchers
                .Where(d => d.Trucks.Count() > 0)
                .ToList()
                .OrderByDescending(d => d.Trucks.Count())
                .ThenBy(d => d.Name)
                .Select(d => new DespatcherExportDto
                {
                    DespatcherName = d.Name,
                    TrucksCount = d.Trucks.Count(),
                    Trucks = d.Trucks
                    .ToList()
                    .OrderBy(t => t.RegistrationNumber)
                    .Select(t => new TruckExportDto
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString()
                    }).ToList()
                }).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<DespatcherExportDto>), new XmlRootAttribute("Despatchers"));

            string result = "";
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                serializer.Serialize(sw, despatchers, ns);

                result = sw.ToString();
            }

            return result;
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
                .Include(c => c.ClientsTrucks)
                .ToList()
                .Select(c => new ClientExportDto()
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                    .Where(ct => ct.Truck.TankCapacity >= capacity)
                    .ToList()
                    .OrderBy(ct => ct.Truck.MakeType.ToString())
                    .ThenByDescending(ct => ct.Truck.CargoCapacity)
                    .Select(ct => new TruckClientExportDto()
                    {
                        TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                        VinNumber = ct.Truck.VinNumber,
                        CargoCapacity = ct.Truck.CargoCapacity,
                        TankCapacity = ct.Truck.TankCapacity,
                        CategoryType = ct.Truck.CategoryType.ToString(),
                        MakeType = ct.Truck.MakeType.ToString(),
                    }).ToList()
                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(c => c.Name)
                .Take(10);

            string json = JsonConvert.SerializeObject(clients, Formatting.Indented);

            return json;
        }
    }
}

namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xml)
        {
            StringBuilder result = new StringBuilder();

            XDocument doc = XDocument.Parse(xml);

            var root = doc.Root;

            var despatchers = root.Elements();

            try
            {
                List<Despatcher> despatcherImp = new List<Despatcher>();
                foreach (var despatcher in despatchers)
                {
                    if(despatcher == null)
                    {
                        throw new Exception();
                    }
                    string name = despatcher.Element("Name").Value;
                    string position = despatcher.Element("Position").Value;

                    var trucks = despatcher.Element("Trucks").Elements();

                    List<Truck> trucksImp = new List<Truck>();
                    foreach (var truck in trucks)
                    {
                        string regNumber = truck.Element("RegistrationNumber").Value;
                        string vinNumber = truck.Element("VinNumber").Value;
                        int tankCapacity = int.Parse(truck.Element("TankCapacity").Value);
                        int cargoCapacity = int.Parse(truck.Element("CargoCapacity").Value);
                        int categoryType = int.Parse(truck.Element("CategoryType").Value);
                        int makeType = int.Parse(truck.Element("MakeType").Value);

                        trucksImp.Add(new Truck
                        {
                            RegistrationNumber = regNumber,
                            VinNumber = vinNumber,
                            TankCapacity = tankCapacity,
                            CargoCapacity = cargoCapacity,
                            CategoryType = (CategoryType)categoryType,
                            MakeType = (MakeType)makeType
                        });
                    }
                    context.Trucks.AddRange(trucksImp);
                    despatcherImp.Add(new Despatcher
                    {
                        Name = name,
                        Position = position,
                        Trucks = trucksImp,
                    });

                    result.AppendLine($"Successfully imported despatcher – {name} with {trucksImp.Count} trucks.");
                }
                int count = despatcherImp.Count;
                context.Despatchers.AddRange(despatcherImp);
                context.SaveChanges();
                
            }
            catch (Exception)
            {

                result.AppendLine("Invalid Data!");
            }

            return result.ToString().Trim();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder result = new StringBuilder();

            try
            {
                List<ClientDto> clientDtos = JsonConvert.DeserializeObject<List<ClientDto>>(jsonString);

                List<Client> clients = new List<Client>();
                foreach (var clientDto in clientDtos)
                {
                    if(clientDto.Type == "usual")
                    {
                        throw new Exception();
                    }

                    clients.Add(new Client()
                    {
                        Name = clientDto.Name,
                        Nationality = clientDto.Nationality,
                        Type = clientDto.Type,
                        ClientsTrucks = clientDto.Trucks
                    });

                    result.AppendLine($"Successfully imported client - {clientDto.Name} with {clientDto.Trucks.Count} trucks.");
                }

                context.Clients.AddRange(clients);
                context.SaveChanges();

            } catch (Exception ex)
            {
                result.AppendLine(ErrorMessage);
            }

            return result.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
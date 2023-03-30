namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .Where(c => c.Footballers.Count() > 0)
                .ToList()
                .Select(c => new CoachExportDto
                {
                    CoachName = c.Name,
                    FootballersCount = c.Footballers.Count(),
                    Footballers = c.Footballers
                    .ToList()
                    .OrderBy(f => f.Name)
                    .Select(f => new FootballerCoachDto
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    }).ToList()
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.CoachName)
                .ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CoachExportDto>), new XmlRootAttribute("Coaches"));

            string result = "";
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                serializer.Serialize(sw, coaches, ns);

                result = sw.ToString();
            }

            return result;
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Include(t => t.TeamsFootballers)
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .ToList()
                .Select(t => new TeamExportDto
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(f => f.Footballer.ContractStartDate >= date)
                    .ToList()
                    .OrderByDescending(f => f.Footballer.ContractEndDate)
                    .ThenBy(f => f.Footballer.Name)
                    .Select(f => new FootballerExportDto
                    {
                        FootballerName = f.Footballer.Name,
                        ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.Footballer.BestSkillType.ToString(),
                        PositionType = f.Footballer.PositionType.ToString()
                    })
                    .ToList()
                })
                .OrderByDescending(t => t.Footballers.Count)
                .ThenBy(t => t.Name)
                .Take(5);

            string json = JsonConvert.SerializeObject(teams, Formatting.Indented);

            return json;
        }
    }
}

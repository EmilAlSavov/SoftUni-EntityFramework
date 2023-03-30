namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder result = new StringBuilder();

            XDocument doc = XDocument.Parse(xmlString);

            int coachesCount = 0;
            int footballersCount = 0;

            var root = doc.Root;

            var coachesXml = root.Elements();

            List<Coach> coaches = new List<Coach>();
            foreach (var coachXml in coachesXml)
            {
                string name = coachXml.Element("Name").Value;
                string nationality = coachXml.Element("Nationality").Value;

                var footballersXml = coachXml.Element("Footballers").Elements();

                CoachDto coach = new CoachDto()
                {
                    Name = name,
                    Nationality = nationality,
                };

                if (!IsValid(coach) || string.IsNullOrWhiteSpace(coach.Name) || string.IsNullOrWhiteSpace(coach.Nationality))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                List<FootballerDto> footballers = new List<FootballerDto>();
                foreach (var footballerXml in footballersXml)
                {
                    string footballerName = footballerXml.Element("Name").Value;
                    string contractStartDateString = footballerXml.Element("ContractStartDate").Value;
                    string contractEndDateString = footballerXml.Element("ContractEndDate").Value;
                    int bestSkillType = int.Parse(footballerXml.Element("BestSkillType").Value);
                    int positionType = int.Parse(footballerXml.Element("PositionType").Value);

                    if(string.IsNullOrEmpty(contractStartDateString) || string.IsNullOrEmpty(contractEndDateString))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime contractStartDate = DateTime.ParseExact(contractStartDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime contractEndDate = DateTime.ParseExact(contractEndDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if(contractStartDate > contractEndDate)
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    FootballerDto footballer = new FootballerDto()
                    {
                        Name = footballerName,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        BestSkillType = (BestSkillType)bestSkillType,
                        PositionType = (PositionType)positionType
                    };

                    if (!IsValid(footballer))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    footballers.Add(footballer);
                    footballersCount++;
                }


                coach.Footballers = footballers;

                coaches.Add(new Coach()
                {
                    Name = coach.Name,
                    Nationality = coach.Nationality,
                    Footballers = coach.Footballers.Select(f => new Footballer
                    {
                        Name = f.Name,
                        ContractStartDate = f.ContractStartDate,
                        ContractEndDate = f.ContractEndDate,
                        BestSkillType = f.BestSkillType,
                        PositionType = f.PositionType,
                    }).ToList()
                });
                result.AppendFormat(SuccessfullyImportedCoach, name, footballers.Count);
                result.AppendLine();
                coachesCount++;
            }
            context.Coaches.AddRange(coaches);  
            context.SaveChanges();
            return result.ToString().Trim();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder result = new StringBuilder();
            var teamsDto = JsonConvert.DeserializeObject<List<TeamDto>>(jsonString);

            int footballersCount = 0;
            List<Team> teams = new List<Team>();
            foreach (var teamDto in teamsDto)
            {
                    Team team = new Team()
                    {
                        Name = teamDto.Name,
                        Nationality = teamDto.Nationality,
                        Trophies = teamDto.Trophies,
                    };

                    if (!IsValid(teamDto) || string.IsNullOrWhiteSpace(team.Name) || string.IsNullOrWhiteSpace(team.Nationality) || team.Trophies <= 0)
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    foreach (var footballerId in teamDto.Footballers)
                    {
                            Footballer? footballer = context.Footballers.Find(footballerId);
                            if (footballer == null)
                            {
                                result.AppendLine(ErrorMessage);
                                continue;
                            }

                            team.TeamsFootballers.Add(new TeamFootballer()
                            {
                                Footballer = footballer,
                                FootballerId = footballerId
                            });
                            footballersCount++;
                    }

                    result.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
                    teams.Add(team);
                
                
            }

            context.Teams.AddRange(teams);
            context.SaveChanges();

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

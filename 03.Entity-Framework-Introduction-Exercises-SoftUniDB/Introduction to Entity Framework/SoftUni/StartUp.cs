using SoftUni.Data;
using SoftUni.Models;
using System.Text;
using System.Linq;

namespace SoftUni
{

    public class StartUp
    {

        static void Main(string[] args)
        {
            SoftUniContext softUniContext = new SoftUniContext();

            string result = RemoveTown(softUniContext);

            Console.WriteLine(result);
        }

        //Ex. 3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            List<Employee> employees = context.Employees.OrderBy(e => e.EmployeeId).ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().Trim();
        }

        //Ex. 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employees = context.Employees.Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
            {
                Firstname = e.FirstName,
                Salary = e.Salary
            }).ToList();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.Firstname} - {employee.Salary:f2}");
            }

            return result.ToString().TrimEnd();
        }

        //Ex. 5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();


            List<Employee> employees = context.Employees.Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary:f2}");
            }

            return result.ToString().TrimEnd();
        }

        //Ex. 6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var content = new StringBuilder();

            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var nakovEmployee = context.Employees
                .First(employee => employee.LastName == "Nakov");

            nakovEmployee.Address = address;

            context.SaveChanges();

            var employeeAddresses = context.Employees
                .OrderByDescending(employee => employee.Address.AddressId)
                .Take(10)
                .Select(employee => employee.Address.AddressText);

            foreach (string employeeAddress in employeeAddresses)
            {
                content.AppendLine(employeeAddress);
            }

            return content.ToString().TrimEnd();
        }

        //Ex. 7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employees = context.Employees
                .Take(10)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                    .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                   .Select(p => new
                   {
                       ProjectName = p.Project.Name,
                       StartDate = p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                       EndDate = p.Project.EndDate.HasValue ? p.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished",
                   }).ToList()
                }).ToList();

            foreach (var e in employees)
            {
                result.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    result.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return result.ToString().Trim();
        }

        //Ex. 8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count())
                .ThenBy(a => a.Town.Name).ThenBy(a => a.AddressText)
                .Take(10).Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count()
                }).ToList();

            foreach (var a in addresses)
            {
                result.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return result.ToString().Trim();
        }

        //Ex. 9
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employee = context.Employees
                .Find(147);

            result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var p in employee.EmployeesProjects.OrderBy(ep => ep.Project.Name))
            {
                result.AppendLine($"{p.Project.Name}");
            }

            return result.ToString().Trim();
        }

        //Ex. 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count())
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
                    .Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        JobTitle = e.JobTitle
                    }).ToList()
                }).ToList();

            foreach (var d in departments)
            {
                result.AppendLine($"{d.DepartmentName} - {d.ManagerFirstName} {d.ManagerLastName}");

                foreach (var e in d.Employees)
                {
                    result.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return result.ToString().Trim();
        }

        //Ex. 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var projects = context.Projects.OrderByDescending(p => p.StartDate)
                .Take(10).Select(p => new
                {
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt")
                }).OrderBy(p => p.Name);

            foreach (var p in projects)
            {
                result.AppendLine($"{p.Name}");
                result.AppendLine($"{p.Description}");
                result.AppendLine($"{p.StartDate}");
            }

            return result.ToString().Trim();
        }

        //Ex. 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var e in employees)
            {
                e.Salary = e.Salary + ((e.Salary / 100) * 12);

                result.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
            }

            context.SaveChanges();

            return result.ToString().Trim();
        }

        //Ex. 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.ToUpper().Substring(0, 2) == "SA")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            foreach (var e in employees)
            {
                result.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
            }

            return result.ToString().Trim();
        }

        //Ex. 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var projectToDel = context.Projects.Find(2);

            context.EmployeesProjects.RemoveRange(context.EmployeesProjects.Where(ep => ep.ProjectId == 2));

            context.Projects.Remove(projectToDel);

            context.SaveChanges();

            var projectsToDisplay = context.Projects.Take(10);

            foreach (var p in projectsToDisplay)
            {
                result.AppendLine($"{p.Name}");
            }

            return result.ToString().Trim();
        }

        //Ex. 15
        public static string RemoveTown(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var townToDel = context.Towns.FirstOrDefault(t => t.Name == "Seattle");
            var addressesInDelTown = context.Addresses.Where(a => a.Town == townToDel);
            int removedAddressesCount = 0;

            if (townToDel != null)
            {
                var employeesInDelTown = context.Employees
                    .Where(e => addressesInDelTown.Any(a => a == e.Address));

                foreach (var employee in employeesInDelTown)
                {
                    employee.AddressId = null;
                }

                context.SaveChanges();

                context.Addresses.RemoveRange(addressesInDelTown);
                removedAddressesCount = context.SaveChanges();

                context.Towns.Remove(townToDel);
                context.SaveChanges();

                result.AppendLine($"{removedAddressesCount} addresses in Seattle were deleted");
            }

            return result.ToString().Trim();
        }

    }
}
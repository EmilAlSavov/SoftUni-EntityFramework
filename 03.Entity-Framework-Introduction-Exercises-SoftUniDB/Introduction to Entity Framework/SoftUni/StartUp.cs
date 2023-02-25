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

            string result = GetEmployeesInPeriod(softUniContext);

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
                .Select(e => e.EmployeesProjects.Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .ToList();
                //.Select(e => new
                //{
                //    FirstName = e.FirstName,
                //    LastName = e.LastName,
                //    ManagerFirstName = e.Manager.FirstName,
                //    ManagerLastName = e.Manager.LastName,
                //    Projects = e.EmployeesProjects.ToList()
                //});

            //foreach (var employee in employees)
            //{
            //    result.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

            //    foreach (var project in employee.Projects)
            //    {
            //        if (project.Project.EndDate != null)
            //        {
            //            result.AppendLine($"--{project.Project.Name} - {project.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - {project.Project.EndDate?.ToString("M/d/yyyy h:mm:ss tt")}");
            //        }
            //        else
            //        {
            //            result.AppendLine($"--{project.Project.Name} - {project.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - not finished");
            //        }
            //    }
            //}

            return result.ToString().Trim();
        }
    }
}
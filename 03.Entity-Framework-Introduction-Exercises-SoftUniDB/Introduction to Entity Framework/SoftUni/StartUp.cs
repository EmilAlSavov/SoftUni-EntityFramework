using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{

    public class StartUp
    {

        static void Main(string[] args)
        {
            SoftUniContext softUniContext = new SoftUniContext();

            string result = GetEmployeesWithSalaryOver50000(softUniContext);

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
    }
}
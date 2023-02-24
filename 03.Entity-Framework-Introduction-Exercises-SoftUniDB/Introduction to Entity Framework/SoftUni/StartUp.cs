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

            string result = GetEmployeesFullInformation(softUniContext);

            Console.WriteLine(result);
        }

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
    }
}
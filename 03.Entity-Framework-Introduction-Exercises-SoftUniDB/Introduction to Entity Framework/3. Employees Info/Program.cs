// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

SoftUniContext context = new SoftUniContext();

using (context)
{
    List<Employee> employees = await context.Employees.OrderBy(e => e.EmployeeId).ToListAsync();

    foreach (var employee in employees)
    {
        Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
    }
}
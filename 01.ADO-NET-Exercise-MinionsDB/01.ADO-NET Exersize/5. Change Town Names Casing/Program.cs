using Microsoft.Data.SqlClient;
using System.Linq;

SqlConnection connection = new SqlConnection("Server =.; Database = MinionsDB; Trusted_Connection = True; Integrated Security = true; Trust Server Certificate=true;");

connection.Open();

string country = Console.ReadLine();
int counter = 0;
List<string> cities = new List<string>();

using (connection)
{
    SqlCommand selectTowns = new SqlCommand(@$"SELECT t.Name 
       FROM Towns as t
       JOIN Countries AS c ON c.Id = t.CountryCode
      WHERE c.Name = '{country}'", connection);

    SqlDataReader selectReader = selectTowns.ExecuteReader();

    if (selectReader.HasRows)
    {

        SqlCommand updateTowns = new SqlCommand($@"UPDATE Towns
   SET Name = UPPER(Name)
 WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = '{country}')", connection);


        while (selectReader.Read())
        {
            counter++;
            cities.Add(selectReader.GetString(0));
        }

        Console.WriteLine($"{counter} town names were affected.");
        Console.WriteLine($"[{String.Join(", ", cities)}]");
    }
    else
    {
        Console.WriteLine("No town names were affected.");
    }
}
// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;

SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDB;Trusted_Connection=True;Integrated Security=true;Trust Server Certificate=true;");

connection.Open();

using (connection)
{
    SqlCommand command = new SqlCommand(@"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
    FROM Villains AS v
    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
GROUP BY v.Id, v.Name
  HAVING COUNT(mv.VillainId) > 3
ORDER BY COUNT(mv.VillainId)", connection);

    SqlDataReader reader = command.ExecuteReader();

    while (reader.Read())
    {
        Console.WriteLine($"{reader.GetString(0)} {reader.GetInt32(1)}");
    }
}
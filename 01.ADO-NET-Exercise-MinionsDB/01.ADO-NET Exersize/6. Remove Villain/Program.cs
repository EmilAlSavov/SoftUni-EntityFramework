using Microsoft.Data.SqlClient;

SqlConnection connection = new SqlConnection("Server =.; Database = MinionsDB; Trusted_Connection = True; Integrated Security = true; Trust Server Certificate=true;");

connection.Open();

int id = int.Parse(Console.ReadLine());
int countMinion = 0;
string villianName = "";
using (connection)
{
    SqlCommand selectVillian = new SqlCommand($"SELECT Name FROM Villains WHERE Id = {id}", connection);
    SqlDataReader villianReader = selectVillian.ExecuteReader();

    if (villianReader.HasRows)
    {
        while (villianReader.Read())
        {
            villianName = villianReader.GetString(0);
        }
        villianReader.Close();

        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            SqlCommand releaseMinions = new SqlCommand($@"DELETE FROM MinionsVillains 
                    WHERE VillainId = {id}", connection, transaction);

            countMinion = releaseMinions.ExecuteNonQuery();

            SqlCommand deleteVillian = new SqlCommand($@"DELETE FROM Villains
      WHERE Id = @villainId", connection);

            Console.WriteLine($"{villianName} was deleted.");
            Console.WriteLine($"{countMinion} minions were released.");
            
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw ex;
        }


    }
    else
    {
        Console.WriteLine("No such villain was found.");
    }
}

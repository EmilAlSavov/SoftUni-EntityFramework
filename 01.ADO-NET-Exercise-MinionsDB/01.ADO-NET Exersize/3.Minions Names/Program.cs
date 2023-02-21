using Microsoft.Data.SqlClient;

SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDB;Trusted_Connection=True;Integrated Security=true;Trust Server Certificate=true;");

connection.Open();

using (connection)
{
    string id = Console.ReadLine();
    SqlCommand villianNameCmd = new SqlCommand($@"SELECT Name FROM Villains WHERE Id = {id}", connection);

    SqlDataReader villianNameReader = villianNameCmd.ExecuteReader();

    if (villianNameReader.HasRows)
    {
        while (villianNameReader.Read())
        {

            Console.WriteLine($"Villian: {villianNameReader.GetString(0)}");


        }
    }
    else
    {
        Console.WriteLine($"No villain with ID {id} exists in the database.");
        return;
    }

    villianNameReader.Close();
    SqlCommand minnionsCmd = new SqlCommand($@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                        m.Name, 
                                        m.Age
                                FROM MinionsVillains AS mv
                                JOIN Minions As m ON mv.MinionId = m.id
                                WHERE mv.VillainId = {id}
                            ORDER BY m.Name", connection);
    SqlDataReader minionsReader = minnionsCmd.ExecuteReader();

        if (minionsReader.HasRows)
        {
            while (minionsReader.Read())
            {
                Console.WriteLine($"{minionsReader.GetInt64(0)}. {minionsReader.GetString(1)} {minionsReader.GetInt32(2)}");
            }
    }
    else
    {
        Console.WriteLine("(no minions)");
    }

}
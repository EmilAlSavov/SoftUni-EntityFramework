using Microsoft.Data.SqlClient;

SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDB;Trusted_Connection=True;Integrated Security=true;Trust Server Certificate=true;");

connection.Open();

string[] inputMinions = Console.ReadLine().Split(' ');
string minionName = inputMinions[1];
int minionAge = int.Parse(inputMinions[2]);
string minionTown = inputMinions[3];
string villianName = Console.ReadLine().Split(' ')[1];

using (connection)
{
    SqlCommand selectTowns = new SqlCommand($"SELECT Id FROM Towns WHERE Name = '{minionTown}'", connection);
    SqlDataReader townReader = selectTowns.ExecuteReader();
    
    if(townReader.HasRows == false)
    {
        SqlCommand insertTown = new SqlCommand($"INSERT INTO Towns (Name) VALUES ('{minionTown}')", connection);
        Console.WriteLine($"Town <TownName> was added to the database.");
    }

    int townId = 0;
    while (townReader.Read())
    {
         townId = townReader.GetInt32(0);
    }
    townReader.Close();

    SqlCommand selectVillians = new SqlCommand($"SELECT Id FROM Villains WHERE Name = '{villianName}'", connection);
    SqlDataReader villianReader = selectVillians.ExecuteReader();

    if (villianReader.HasRows == false)
    {
        SqlCommand insertVillians = new SqlCommand($"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES ('{villianName}', 4)", connection);
        Console.WriteLine($"Villain {villianName} was added to the database.");
    }

    int villianId = 0;
    while (villianReader.Read())
    {
        villianId = villianReader.GetInt32(0);
    }
    villianReader.Close();

    SqlCommand insertMinnion = new SqlCommand($"INSERT INTO Minions (Name, Age, TownId) VALUES ('{minionName}', {minionAge}, ${townId})", connection);

    SqlCommand selectMinion = new SqlCommand($"SELECT Id FROM Minions WHERE Name = '{minionName}'", connection);
    SqlDataReader minnReader = selectMinion.ExecuteReader();

    int minionId = 0;
    while (minnReader.Read())
    {
        minionId = minnReader.GetInt32(0);
    }
    SqlCommand insertMinionVillian = new SqlCommand($"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES ({minionId}, {villianId})", connection);
    Console.WriteLine($"Successfully added {minionName} to be minion of {villianName}.");
}
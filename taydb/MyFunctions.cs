namespace MyProject;
using System.Data.SqlClient;
public class MyFunctions
{
    public static bool TestSQLConnection(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.Beep();
                connection.Close();
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }
    }

    public static void AddAlbum(string connectionString, string Name, string Artist)
    {
        bool SqlState = TestSQLConnection(connectionString);

        if (!SqlState)
        {
            Console.WriteLine("Unable to connect to SQL Server");
            return;
        }
        
        string SQLQuery = "INSERT INTO [dim].[album] ([name], [artist]) VALUES (@AlbumName, @ArtistName)";

        using SqlCommand command = new SqlCommand(SQLQuery, new SqlConnection(connectionString));
        command.Parameters.AddWithValue("@AlbumName", Name);
        command.Parameters.AddWithValue("@ArtistName", Artist);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }
        /// <summary>
        /// Querys the SQL Server database to get all albums in the base
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
    public static void GetAlbums(string connectionString)
    {
        string sqlQuery = "SELECT [id], [name], [artist] FROM [dim].[album];";

        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(sqlQuery, connection);
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string artist = reader.GetString(2);
                Thread.Sleep(2000);
                Console.Beep();
                Console.WriteLine($"ID: {id}, Name: {name}, Artist: {artist}");
            }
        }
        Console.ReadLine();
    }

    public static void AddMusic(string connectionString, string Music, string Artist, string Album, int Duration)
    {
        bool SqlState = TestSQLConnection(connectionString);

        if (!SqlState)
        {
            Console.WriteLine("Unable to connect to SQL Server");
            return;
        }
        
        string SQLQuery = "EXEC [add_music] " +
            "@SongName = @SelfName, " +
            "@ArtistName = @SelfArtist, " +
            "@AlbumName = @SelfAlbum, " +
            "@Duration = @SelfDuration, " +
            "@rating = @SelfRating, " +
            "@status = @SelfStatus";

        using SqlCommand command = new SqlCommand(SQLQuery, new SqlConnection(connectionString));
        command.Parameters.AddWithValue("@SelfName", Music);
        command.Parameters.AddWithValue("@SelfArtist", Artist);
        command.Parameters.AddWithValue("@SelfAlbum", Album);
        command.Parameters.AddWithValue("@SelfDuration", Duration);
        command.Parameters.AddWithValue("@SelfRating", 0);
        command.Parameters.AddWithValue("@SelfStatus", 1);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            string result = reader.GetString(0);
            if (result is not null && result is string)
            {
                Console.WriteLine(result);
            }
        }
    }
}

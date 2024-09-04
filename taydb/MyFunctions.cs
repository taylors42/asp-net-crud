namespace MyProject;

using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
public class MyFunctions
{
    public static void TestSQLConnection(string connectionString)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();
            Console.Beep();
            connection.Close();
        }

        catch (Exception)
        {
            Console.WriteLine("Unable to connect to SQL Server");
            Console.ReadLine();
        }
    }

        /// <summary>
        /// Insert a music album into the SQL Server album dimension database
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="Name">The name of the album to insert</param>
        /// <param name="Artist">The artist of the album to insert</param>
    public static void AddAlbum(string connectionString, string Name, string Artist)
    {
        TestSQLConnection(connectionString);
        
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
        TestSQLConnection(connectionString);

        string sqlQuery = "SELECT TOP 100 [id], [name], [artist] FROM [dim].[album];";

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
                Console.WriteLine($"ID: {id} | Name: {name} | Artist: {artist}");
            }
        }
        Console.WriteLine(" ");
        Console.WriteLine("Done");
        Console.ReadLine();
    }

        /// <summary>
        /// Insert a music into the SQL Server music dimension database
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="Music">The name of the music to insert</param>
        /// <param name="Artist">The artist of the music to insert</param>
        /// <param name="Album">The album of the music to insert</param>
        /// <param name="Duration">The duration of the music to insert</param>
        /// <remarks>
        /// The Album most be a valid and existent album
        /// </remarks>
        /// <returns> returns a table named [result] with the explanation of the result </returns>
    public static void AddMusic(string connectionString, string Music, string Artist, string Album, int Duration)
    {
        TestSQLConnection(connectionString);
        
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
            if (result is not null)
            {
                Console.WriteLine(result);
            }
        }
    }

public static void GetMusics(string connectionString, string SelfAlbum)
{
    TestSQLConnection(connectionString);
    var musicIds = new List<int>();
    var musics = new List<string>();

    string SQLQueryToGetIds = "EXEC [get_musics_by_album] @album = @SelfAlbum";
    string SQLQueryToGetMusics = "SELECT [name], [artist], [album], [duration] FROM [dim].[song] WHERE [id] = @id;";

    using SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();

    using SqlCommand commandToGetIds = new SqlCommand(SQLQueryToGetIds, connection);
    commandToGetIds.Parameters.AddWithValue("@SelfAlbum", SelfAlbum);

    using SqlDataReader readerToGetIds = commandToGetIds.ExecuteReader();
    while (readerToGetIds.Read())
    {
        int result = readerToGetIds.GetInt32(0);
        musicIds.Add(result);
    }
    readerToGetIds.Close();

    using SqlCommand commandToGetMusics = new SqlCommand(SQLQueryToGetMusics, connection);
    foreach (int id in musicIds)
    {
        commandToGetMusics.Parameters.Clear();
        commandToGetMusics.Parameters.AddWithValue("@id", id);

        using SqlDataReader readerToGetMusics = commandToGetMusics.ExecuteReader();
        while (readerToGetMusics.Read())
        {
            string musicName = readerToGetMusics.GetString(0);
            string musicArtist = readerToGetMusics.GetString(1);
            string music = $"Name: {musicName}, Artist: {musicArtist}";
            musics.Add(music);
        }
        readerToGetMusics.Close();
    }

    connection.Close();

    foreach (string music in musics)
    {
        Console.WriteLine(music);
    }

    Console.WriteLine("Done");
    Console.ReadLine();
}
}

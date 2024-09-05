using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace SoundBox;

public class MusicManager
{

    /// <summary>
    /// Test the connection on the SQL Server
    /// </summary>
    public static void TestSQLConnection()
    {
        using SqlConnection connection = new(connectionString);
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
    public static void AddAlbum(string name, string artist)
    {
        TestSQLConnection();
        
        string SQLQuery = "INSERT INTO [dim].[album] ([name], [artist]) VALUES (@AlbumName, @ArtistName)";

        using SqlCommand command = new(SQLQuery, new SqlConnection(connectionString));
        command.Parameters.AddWithValue("@AlbumName", name);
        command.Parameters.AddWithValue("@ArtistName", artist);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }
    /// <summary>
    /// Querys the SQL Server database to get all albums in the base
    /// </summary>
    /// <param name="connectionString">The connection string to use</param>
    public static void GetAlbums()
    {
        TestSQLConnection();
        var albums = new List<string>();

        string sqlQuery = "SELECT TOP 100 [name], [artist] FROM [dim].[album];";

        using SqlConnection connection = new(connectionString);
        using SqlCommand command = new(sqlQuery, connection);
        connection.Open();
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            string name = reader.GetString(0);
            string artist = reader.GetString(1);
            albums.Add($"Name: {name} | Artist: {artist}");
        };

        reader.Close();
        connection.Close();

        foreach (var album in albums)
        {
            Thread.Sleep(1000);
            Console.Beep();
            Console.WriteLine(album);
        };

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
    public static void AddMusic(string music, string artist, string album, int duration)
    {
        TestSQLConnection();
        
        string SQLQuery = 
            "EXEC [add_music] " +
            "@name = @paramName, " +
            "@artist = @paramArtist, " +
            "@album = @paramAlbum, " +
            "@duration = @paramDuration, " +
            "@rating = @paramRating, " +
            "@status = @paramStatus";

        using SqlCommand command = new(SQLQuery, new SqlConnection(connectionString));

        command.Parameters.AddWithValue("@paramName", music);
        command.Parameters.AddWithValue("@paramArtist", artist);
        command.Parameters.AddWithValue("@paramAlbum", album);
        command.Parameters.AddWithValue("@paramDuration", duration);
        command.Parameters.AddWithValue("@paramRating", 0);
        command.Parameters.AddWithValue("@paramStatus", 1);

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

        reader.Close();

    }

    // public static void RemoveAlbum(string album)
    // {
    //     TestSQLConnection();

    //     string sqlQueryCheckIfExist = "SELECT [id] FROM [dim].[album] WHERE [name] = @Album;";
    //     string sqlQueryDelete = "DELETE FROM [dim].[album] WHERE [name] = @Album;";
    //     int id = 0;
    //     using SqlConnection connection = new(connectionString);
    //     connection.Open();
    //     using SqlCommand command = new(sqlQueryCheckIfExist, connection);

    //     using SqlDataReader readerOfExistence = command.ExecuteReader();

    //     while (readerOfExistence.Read())
    //     {

    //     }

    //     Console.WriteLine(" ");
    //     Console.WriteLine("Done");
    //     Console.ReadLine();
    // }

    public static void GetMusics(string Album)
    {
        TestSQLConnection();

        var musicIdList = new List<int>();
        var musicList = new List<string>();

        string SQLQueryToGetIds = "EXEC [get_musics_by_album] @album = @Album";
        string SQLQueryToGetMusics = "SELECT [name], [artist], [album], [duration] FROM [dim].[song] WHERE [id] = @id;";

        using SqlConnection connection = new(connectionString);
        connection.Open();

        using SqlCommand commandToGetIds = new(SQLQueryToGetIds, connection);
        commandToGetIds.Parameters.AddWithValue("@Album", Album);

        using SqlDataReader readerToGetIds = commandToGetIds.ExecuteReader();

        while (readerToGetIds.Read())
        {
            int result = readerToGetIds.GetInt32(0);
            musicIdList.Add(result);
        }

        readerToGetIds.Close();

        using SqlCommand commandToGetMusics = new(SQLQueryToGetMusics, connection);

        foreach (int id in musicIdList)
        {
            commandToGetMusics.Parameters.Clear();
            commandToGetMusics.Parameters.AddWithValue("@id", id);

            using SqlDataReader readerToGetMusics = commandToGetMusics.ExecuteReader();
            while (readerToGetMusics.Read())
            {
                string musicName = readerToGetMusics.GetString(0);
                string musicArtist = readerToGetMusics.GetString(1);
                string music = $"Name: {musicName}, Artist: {musicArtist}";
                musicList.Add(music);
            }
            readerToGetMusics.Close();
        }

        connection.Close();

        foreach (string music in musicList)
        {
            Console.WriteLine(music);
        }

        Console.WriteLine("Done");
        Console.ReadLine();
    }
}

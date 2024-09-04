using System;
using System.Collections.Generic;
using static MyProject.MyFunctions;
namespace MyProject;
public class Utilitaries
{
    private static string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")!;

    private static readonly Dictionary<string, Action> options = new()
    {
        { "Add Album", AddAlbumAction },
        { "Add Music", AddMusicAction},
        { "Show Albums", ShowAlbumsAction},
        { "Quit", ByeAction},
    };


    public static void AddMusicAction()
    {
        Console.Clear();
        Console.Beep();
        Console.Write("Write the name of the music: ");
        string songName = Console.ReadLine()!;
        Console.Clear();

        Console.Write("Write the name of the artist: ");
        string artistName = Console.ReadLine()!;

        Console.Write("Write the name of the album: ");
        string albumName = Console.ReadLine()!;

        Console.WriteLine("Write the duration in seconds: ");
        int duration = int.Parse(Console.ReadLine()!);
    }

    public static void AddAlbumAction()
    {
        Console.Clear();
        Console.Beep();
        Console.Write("Write the name of the album: ");
        string albumName = Console.ReadLine()!;

        Console.Write("Write the name of the artist: ");
        string artistName = Console.ReadLine()!;

        AddAlbum(connectionString, albumName, artistName);
    }

    public static void ShowAlbumsAction()
    {
        Console.Clear();
        Console.Beep();
        GetAlbums(connectionString);
    }

    public static void ShowMusicsAction()
    {
        Console.Clear();
        Console.Beep();
        Console.Write("Write the name of the album: ");
        string albumName = Console.ReadLine()!;

        GetMusics(connectionString, albumName);
    }

    public static void ByeAction()
    {
        Console.Beep();
        Environment.Exit(0);
    }

    public static void ShowMenuOptions()
    {
        var options = new Dictionary<string, Action>
        {
            { "Add Album", AddAlbumAction },
            { "Add Music", AddMusicAction},
            { "Show Albums", ShowAlbumsAction},
            { "Show Musics", ShowMusicsAction},
            { "Quit", ByeAction},
        };

        var optionKeys = new List<string>(options.Keys);
        int optionIndex = 0;

        while (true)
        {
            Console.Clear();

            for (int i = 0; i < optionKeys.Count; i++)
            {
                if (i == optionIndex)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write("  ");
                }

                Console.WriteLine(optionKeys[i]);
            }

            ConsoleKeyInfo userKey = Console.ReadKey();

            switch (userKey.Key)
            {
                case ConsoleKey.DownArrow:
                    optionIndex = (optionIndex + 1) % optionKeys.Count;
                    Console.Beep();
                    break;

                case ConsoleKey.UpArrow:
                    optionIndex = (optionIndex - 1 + optionKeys.Count) % optionKeys.Count;
                    Console.Beep();

                    break;

                case ConsoleKey.Enter:
                    options[optionKeys[optionIndex]].Invoke();
                    break;

                case ConsoleKey.Escape:
                    ByeAction();
                    break;
                
                case ConsoleKey.Q:
                    ByeAction();
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    Console.ReadKey();
                    break;
            }
        }
    }
}

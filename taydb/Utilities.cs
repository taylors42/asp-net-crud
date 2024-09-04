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
        { "Show Musics", ShowMusicsAction},
        { "Get Musics", RemoveAlbumAction},
        { "Quit", ByeAction},
    };


    private static void AddMusicAction()
    {
        Console.Clear();
        Console.Beep();

        Console.Write("Write the name of the music: ");
        string songName = Console.ReadLine()!;
        
        Console.Clear();
        Console.Beep();

        Console.Write("Write the name of the artist: ");
        string artistName = Console.ReadLine()!;

        Console.Clear();
        Console.Beep();

        Console.Write("Write the name of the album: ");
        string albumName = Console.ReadLine()!;

        Console.Clear();
        Console.Beep();

        Console.WriteLine("Write the duration in seconds: ");
        int duration = int.Parse(Console.ReadLine()!);

        AddMusic(connectionString, songName, artistName, albumName, duration);
    }

    private static void AddAlbumAction()
    {
        Console.Clear();
        Console.Beep();
        Console.Write("Write the name of the album: ");
        string albumName = Console.ReadLine()!;

        Console.Write("Write the name of the artist: ");
        string artistName = Console.ReadLine()!;

        AddAlbum(connectionString, albumName, artistName);
    }

    private static void ShowAlbumsAction()
    {
        Console.Clear();
        Console.Beep();

        GetAlbums(connectionString);
    }

    private static void ShowMusicsAction()
    {
        Console.Clear();
        Console.Beep();

        Console.Write("Write the name of the album: ");
        string albumName = Console.ReadLine()!;

        GetMusics(connectionString, albumName);
    }

    private static void RemoveAlbumAction()
    {
        Console.Clear();
        Console.Beep();

        Console.Write("Write the name of the album: ");
        string albumName = Console.ReadLine()!; 
        RemoveAlbum(connectionString, albumName);
    }

    private static void ByeAction()
    {
        Console.Clear();
        Console.Beep();
        Environment.Exit(0);
    }

    public static void ShowMenuOptions()
    {
        var optionKeys = new List<string>(options.Keys);
        int optionIndex = 0;

        while (true)
        {
            Console.Clear();

            for (int i = 0; i < optionKeys.Count; i++)
            {
                if (i == optionIndex)
                {
                    Console.Write("* ");
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

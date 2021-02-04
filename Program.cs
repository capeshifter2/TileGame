using System;
using SadConsole;
using Console = SadConsole.Console;
using NAudio.Wave;
using System.Threading;

namespace TileGame
{
    class Program
    {

        public const int Width = 80;
        public const int Height = 40;

        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        static void Init()
        {
            MapScreen screen = new MapScreen(Width, Height);
            Thread songplayer = new Thread(()=>PlaySong(screen));
            
            songplayer.Start();
            screen.IsFocused = true;
            Global.CurrentScreen = screen;
            

        }
        public static void PlaySong(MapScreen screen)
        {
            while (true)
            {
                
                using (var gameMusic = new AudioFileReader(@"C:\Users\CapeS\source\repos\TileGame\GameSong.wav"))
                using (var menuMusic = new AudioFileReader(@"C:\Users\CapeS\source\repos\TileGame\MenuSong.wav"))
                using (var outputDevice = new WaveOutEvent())
                {
                    if (screen.currentlyViewing == "Game")
                    {
                        outputDevice.Init(gameMusic);
                        outputDevice.Play();
                        while (outputDevice.PlaybackState == PlaybackState.Playing && screen.currentlyViewing == "Game")
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    else if (screen.currentlyViewing == "Main Menu")
                    {
                        outputDevice.Init(menuMusic);
                        outputDevice.Play();
                        while (outputDevice.PlaybackState == PlaybackState.Playing && screen.currentlyViewing == "Main Menu")
                        {
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }
    }
    
    
}


using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using Console = SadConsole.Console;
using Keyboard = SadConsole.Input.Keyboard;
using NAudio.Wave;

class MapScreen : ContainerConsole
{
    public Console MapConsole { get; set; }
    double monstersLastActed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    double playerLastActed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    Player player;
    public Cell PlayerGlyph { get; set; }
    private Point _playerPosition;
    private Map floor = new Map();
    private int roomNum = -1;
    public string currentlyViewing = "Main Menu";
    public Point PlayerPosition
    {
        get => _playerPosition;
        private set
        {
            MapConsole.Print(25,25, PlayerGlyph.ToString());
            MapConsole.Clear(_playerPosition.X, _playerPosition.Y);
            _playerPosition = value;
            PlayerGlyph.CopyAppearanceTo(MapConsole[_playerPosition.X, _playerPosition.Y]);
        }
    }
    public MapScreen(int mapConsoleWidth, int mapConsoleHeight)
    {
         

        // Setup map
        MapConsole = new Console(mapConsoleWidth, mapConsoleHeight);
        //MapConsole.DrawBox(new Microsoft.Xna.Framework.Rectangle(0, 0, MapConsole.Width, MapConsole.Height), new Cell(Color.White, Color.DarkGray, 0));
        MapConsole.Parent = this;

        // Setup player 

        player = new Player();
        player.RandomName();
        
        PlayerGlyph = new Cell(Color.White, Color.Black, 1);
        _playerPosition = new Point(2, 2);
        PlayerGlyph.CopyAppearanceTo(MapConsole[_playerPosition.X, _playerPosition.Y]);
        NextFloor();
    }

    public override void Update(TimeSpan timeElapsed)
    {
        if (currentlyViewing == "Game")
        {
            Room currentRoom = floor.GetRoom(roomNum);
            player.x = PlayerPosition.X;
            player.y = PlayerPosition.Y;
            for (int i = -1; i < 1; i++) 
            {
                for (int j = -1; j < 1; j++)
                    currentRoom.grid[player.x + i, player.y + j].isSeen = true;
            }
            if (player.health <= 0)
            {
                MapConsole.Clear();
                MapConsole.Print(5, 10, "You Died", Color.DarkRed);
                currentlyViewing = "Dead";
                Thread playsound = new Thread(() => PlayEffect(@"C:\Users\CapeS\source\repos\TileGame\DeathSong.wav"));
                playsound.Start();
                return;
            }
            if (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - monstersLastActed >= 200)
            {
                monstersLastActed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                MonstersAct();
            }
            if (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - playerLastActed >= 100)
            {
                playerLastActed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                CheckKey();
            }


            MapConsole.Print(1, 26, "Your hero is: " + player.name, Color.White);
            MapConsole.Print(1, 27, "Health: " + player.health + "/" + player.maxHealth + "       " + PlayerPosition.X + " " + PlayerPosition.Y + " ", Color.White);
            DrawMap();
        }
        else if (currentlyViewing == "Main Menu")
        {
            MapConsole.Clear();
            MapConsole.Print(35, 4, "(S)tart", Color.LightGreen);
            MapConsole.Print(35, 8, "(Q)uit", Color.Red);
            if (Global.KeyboardState.IsKeyDown(Keys.S))
            {
                currentlyViewing = "Game";
                MapConsole.Clear();

            }
            else if (Global.KeyboardState.IsKeyDown(Keys.Q))
            {
                Environment.Exit(0);
            }
        }
        else if (currentlyViewing == "Dead")
        {
            MapConsole.Clear();
            MapConsole.Print(35, 4, "Re(S)tart", Color.LightGreen);
            MapConsole.Print(35, 7, "(Q)uit", Color.Red);
            MapConsole.Print(35, 10, "You Died", Color.DarkRed);
            if (Global.KeyboardState.IsKeyDown(Keys.S))
            {
                MapScreen newScreen = new MapScreen(80, 40);
                newScreen.IsFocused = true;
                Thread songPlayer = new Thread(()=>PlaySong(newScreen));
                songPlayer.Start();
                Global.CurrentScreen = newScreen;

            }
            else if (Global.KeyboardState.IsKeyDown(Keys.Q))
            {
                Environment.Exit(0);
            }
        }
        else if (currentlyViewing == "Victory")
        {
            MapConsole.Clear();
            MapConsole.Print(35, 10, "You Won!", Color.LightSkyBlue);
            MapConsole.Print(35, 4, "(M)ain Menu", Color.LightGreen);
            MapConsole.Print(35, 7, "(Q)uit", Color.Red);
            if (Global.KeyboardState.IsKeyDown(Keys.M))
            {
                MapScreen newScreen = new MapScreen(80, 40);
                newScreen.IsFocused = true;
                Thread songPlayer = new Thread(() => PlaySong(newScreen));
                songPlayer.Start();
                Global.CurrentScreen = newScreen;

            }
            else if (Global.KeyboardState.IsKeyDown(Keys.Q))
            {
                Environment.Exit(0);
            }
        }
        
    }

    public void CheckKey()
    {
        
        Room currentRoom = floor.GetRoom(roomNum);
        Keyboard info = Global.KeyboardState;
        Point newPlayerPosition = PlayerPosition;
        if (info.IsKeyDown(Keys.W))
        {
            newPlayerPosition += Directions.North;
            if (currentRoom.CheckMovable(newPlayerPosition.X, newPlayerPosition.Y) == "Monster")
            {
                if (currentRoom.CheckIfMonster(newPlayerPosition.X, newPlayerPosition.Y))
                {
                    PlayerAttack(newPlayerPosition.X, newPlayerPosition.Y);
                }
                newPlayerPosition -= Directions.North;
            }
            else if (currentRoom.CheckMovable(newPlayerPosition.X, newPlayerPosition.Y) == "Door")
            {
                NextFloor();
            }

        }
        else if (info.IsKeyDown(Keys.S))
        {
            newPlayerPosition += Directions.South;
            if (currentRoom.CheckMovable(newPlayerPosition.X, newPlayerPosition.Y) == "Monster")
            {
                if (currentRoom.CheckIfMonster(newPlayerPosition.X, newPlayerPosition.Y))
                {
                    PlayerAttack(newPlayerPosition.X, newPlayerPosition.Y);
                }
                newPlayerPosition -= Directions.South;
            }
            else if (currentRoom.CheckMovable(newPlayerPosition.X, newPlayerPosition.Y) == "Door")
            {
                NextFloor();
            }

        }
        else if (info.IsKeyDown(Keys.A))
        {
            newPlayerPosition += Directions.West;
            if (currentRoom.CheckMovable(newPlayerPosition.X, newPlayerPosition.Y) == "Monster")
            {
                if (currentRoom.CheckIfMonster(newPlayerPosition.X, newPlayerPosition.Y))
                {
                    PlayerAttack(newPlayerPosition.X, newPlayerPosition.Y);
                }
                newPlayerPosition -= Directions.West;
            }
            else if (currentRoom.CheckMovable(newPlayerPosition.X, newPlayerPosition.Y) == "Door")
            {
                NextFloor();
            }

        }
        else if (info.IsKeyDown(Keys.D))
        {
            newPlayerPosition += Directions.East;
            if (currentRoom.CheckMovable(newPlayerPosition.X, newPlayerPosition.Y) == "Monster")
            {
                if (currentRoom.CheckIfMonster(newPlayerPosition.X, newPlayerPosition.Y))
                {
                    PlayerAttack(newPlayerPosition.X, newPlayerPosition.Y);
                }
                newPlayerPosition -= Directions.East;
            }
            else if (currentRoom.CheckMovable(newPlayerPosition.X, newPlayerPosition.Y) == "Door")
            {
                NextFloor();
            }
            
        }
        if (newPlayerPosition != PlayerPosition && currentRoom.grid[newPlayerPosition.X, newPlayerPosition.Y].getTileType() == MapTile.TileType.Floor)
        {
            PlayerPosition = newPlayerPosition;
            return;
        }

        return;
    }
    private void NextFloor()
    {
        MapConsole.Clear();
        if (roomNum < 4)
            roomNum++;
        else
        {
            MapConsole.Print(35, 10, "You Won!", Color.Green);
            currentlyViewing = "Victory";
            return;
        }
        PlayerPosition = new Point(2, 2);   
        Room currentRoom = floor.GetRoom(roomNum);
        MapConsole.Clear();

    }
    private Cell GetCell(int i, int j)
    {
        Room currentRoom = floor.GetRoom(roomNum);
        
        
        if (currentRoom.grid[i, j] == null)
        {
            return new Cell(Color.Black);
        }

        MapTile tile = currentRoom.GetTile(new Location(i, j, roomNum));
        Monster tempMonster;

        if (currentRoom.CheckIfMonster(i, j, out tempMonster))
            return new Cell(Color.Green, Color.Black, 77);
        else if (tile.getTileType() == MapTile.TileType.Floor)
            return new Cell(Color.Black);
        else if (tile.getTileType() == MapTile.TileType.Door)
            return new Cell(Color.White, Color.Black, 124);
        else if (tile.getTileType() == MapTile.TileType.Wall)
            return new Cell(Color.Black, Color.White, 178);
        return new Cell(Color.Black);
    }
    private void MonstersAct()
    {

        Room currentRoom = floor.GetRoom(roomNum);
        for (int i = 0; i < currentRoom.monsters.Count; i++)
        {
            if (currentRoom.monsters[i].Act(player, currentRoom))
            {
                Thread thread = new Thread(()=>FlickerGlyph());
                thread.Start();
            }
            
            
        }
    }
    private void PlayerAttack(int x, int y)
    {
        Room currentRoom = floor.GetRoom(roomNum);
        Random ranGen = new Random();
        Monster monsterAttacked;
        currentRoom.CheckIfMonster(x, y, out monsterAttacked);
        monsterAttacked.ModifyHealth(player.Attack());
        MapConsole.Print(1, 28, "Monster Health:" + monsterAttacked.health.ToString() + "       ", Color.White);
        if (monsterAttacked.health <= 0)
        {
            currentRoom.monsters.Remove(monsterAttacked);
            for (int i = 0; i < currentRoom.monsters.Count; i++)
            {
                currentRoom.monsters[i].index = i;
            }
        }
    }
    private void PlayEffect(string path)
    {
        using (var effectSound = new AudioFileReader(path))
        using (var outputDevice = new WaveOutEvent())
        {
            outputDevice.Init(effectSound);
            outputDevice.Play();
            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(1000);
            }
        }
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
    private void FlickerGlyph()
    {
        PlayerGlyph = new Cell(Color.Red, Color.Black, 1);
        Thread.Sleep(100);
        PlayerGlyph = new Cell(Color.White, Color.Black, 1);
    }
    private bool IsInSight(int x, int y)
    {
        if (x < player.x + 3 && x > player.x - 3)
        {
            if (y < player.y + 3 && y > player.y - 3)
            {
                return true;
            }
        }
        return false;
    }
    private void DrawMap()
    {
        Room currentRoom = floor.GetRoom(roomNum);
        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                if (currentRoom.grid[i, j].IsSeen())
                    MapConsole.DrawBox(new Rectangle(i + 1, j + 1, 0, 0), GetCell(i, j));
                
            }
        }
        PlayerGlyph.CopyAppearanceTo(MapConsole[PlayerPosition.X, PlayerPosition.Y]);
    }


}

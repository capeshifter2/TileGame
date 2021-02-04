using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Room
{
    public MapTile[,] grid { get; }
    int x;
    int y;
    public List<Monster> monsters = new List<Monster>();
	public Room(string path)
	{
        
        grid = new MapTile[100, 100];
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                MapTile tile = new MapTile();
                tile.setTileType(MapTile.TileType.Floor);


                grid[i, j] = tile;
            }
        }
        using (StreamReader sr = new StreamReader(path))
        {
            char nextchar;
            int nextcharnum;
            y = 0;
            x = 0;
            while ((nextcharnum = sr.Read()) != -1) {
                nextchar = (char)nextcharnum;
               // Console.Write(nextchar);
                if (nextchar == '#')
                {
                    MapTile tempTile = new MapTile();
                    tempTile.setTileType(MapTile.TileType.Wall);
                    grid[x,y] = tempTile;
                    x++;
                     
                }
                else if (nextchar == ' ')
                {
                    MapTile tempTile = new MapTile();
                    tempTile.setTileType(MapTile.TileType.Floor);
                    grid[x, y] = tempTile;
                    x++;
                }
                else if (nextchar == 'M')
                {
                    MapTile tempTile = new MapTile();
                    tempTile.setTileType(MapTile.TileType.Floor);
                    Monster tempMonster = new Monster();
                    tempMonster.x = x;
                    tempMonster.y = y;
                    tempMonster.index = monsters.Count();
                    monsters.Add(tempMonster);
                    grid[x, y] = tempTile;
                    x++;
                }
                else if (nextchar == '|')
                {
                    MapTile tempTile = new MapTile();
                    tempTile.setTileType(MapTile.TileType.Door);
                    grid[x, y] = tempTile;
                    x++;
                }
                else if (nextchar == '\n')
                {
                    x = 0;
                    y++;
                }
            }
            

            
            //Console.Write(x + " " + y + "\n");
        }
    }
    public MapTile GetTile(Location location)
    {
        int y, x;
        location.GetLocation(out x, out y);
       
        return grid[x,y];
    }
    public void SetTile(int x, int y, MapTile.TileType type)
    {
        grid[x,y].setTileType(type);
    }

    private MapTile CreateTile(MapTile.TileType type)
    {
        return new MapTile(type);
    }
    public void GetSize(out int x, out int y)
    {
        x = this.y;
        y = this.x;
    }
    public String CheckMovable(int x, int y)
    {
        if (CheckIfMonster(x, y))
        {
            return "Monster";
        }
        else if (grid[x,y].getTileType() == MapTile.TileType.Floor)
        {
            return "Floor";
        }
        else if (grid[x, y].getTileType() == MapTile.TileType.Door)
        {
            return "Door";
        }
        else
        {
            return "No";
        }
    }
    public bool CheckIfMonster(int x, int y)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i].x == x && monsters[i].y == y)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckIfMonster(int x, int y, out Monster monster)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i].x == x && monsters[i].y == y)
            {
                monster = monsters[i];
                return true;
            }
        }
        monster = new Monster();
        return false;
    }

}


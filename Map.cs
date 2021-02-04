using System;

public class Map
{
    Room[] rooms;
    public Map()
    {
        rooms = new Room[5];
        for (int i = 0; i < 5; i++)
        {
            
            rooms[0] = new Room(@"C:\Users\CapeS\source\repos\TileGame\Map1.1.txt");
            rooms[1] = new Room(@"C:\Users\CapeS\source\repos\TileGame\Map1.2.txt");
            rooms[2] = new Room(@"C:\Users\CapeS\source\repos\TileGame\Map1.3.txt");
            rooms[3] = new Room(@"C:\Users\CapeS\source\repos\TileGame\Map1.4.txt");
            rooms[4] = new Room(@"C:\Users\CapeS\source\repos\TileGame\Map1.5.txt");
        }
    }
    public void SetTile(Location location, MapTile.TileType type)
    {
        int x;
        int y;
        int roomNum;
        location.GetLocation(out x, out y, out roomNum);
        rooms[roomNum].SetTile(x, y, type);
    }
    public MapTile GetTile(Location location)
    {

        location.GetLocation(out int roomnum);
        
        return rooms[roomnum].GetTile(location);
        
    }
    public Room GetRoom(int roomnum)
    {
        return rooms[roomnum];
    }

}

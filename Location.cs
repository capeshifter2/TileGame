using System;

public class Location
{
	int x;
	int y;
	int roomnum;
	public Location(int x, int y, int roomnum)
	{
		this.x = x;
		this.y = y;
		this.roomnum = roomnum;
	}
	public void GetLocation(out int x, out int y, out int roomnum)
    {
		x = this.x;
		y = this.y;
		roomnum = this.roomnum;
    }
	public void GetLocation(out int x, out int y)
	{
		x = this.x;
		y = this.y;
	}
	public void GetLocation(out int roomnum)
	{
		roomnum = this.roomnum;

	}
	public int GetRoomnum()
    {
		
		return this.roomnum;
    }
	public bool Equals(Location location)
    {
		if (location == null)
        {
			return false;
        }
		location.GetLocation(out int x, out int y, out int roomnum);
		if (this.x == x && this.y == y && this.roomnum == roomnum)
        {
			return true;
        }
        else
        {
			return false;
        }
    }
}

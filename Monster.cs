using System;
using System.Threading;

public class Monster
{
	//Use as template for future monsters
	public int health;
	public int index;
	public int x;
	public int y;
	
	Random ranGen = new Random();
	public Monster()
	{
		health = ranGen.Next(10, 20);
	}
	public void Attack(Player player)
    {
		int modBy = ranGen.Next(1, 3) * -1;
		
		player.ModifyHealth(modBy);
    }
	public bool Act(Player player, Room currentRoom)
    {
		if (player.y < y - 1 && currentRoom.CheckMovable(x, y - 1) == "Floor")
		{
			y--;
		}
		else if (player.y > y + 1 && currentRoom.CheckMovable( x, y + 1) == "Floor")
        {
			y++;
        }
		if (player.x > x + 1 && currentRoom.CheckMovable(x + 1, y) == "Floor")
		{
			x++;
		}
		else if (player.x < x - 1 && currentRoom.CheckMovable(x - 1, y) == "Floor")
		{
			x--;
		}
		else if (player.x == x - 1 && player.y == y - 1)
        {
			if (currentRoom.CheckMovable(x, y - 1) == "Floor")
            {
				y--;
            }
			else if (currentRoom.CheckMovable(x - 1, y) == "Floor" )
            {
				x--;
            }
        }
		else if (player.x == x + 1 && player.y == y - 1)
		{
			if (currentRoom.CheckMovable(x, y - 1) == "Floor")
			{
				y--;
			}
			else if (currentRoom.CheckMovable(x + 1, y) == "Floor")
			{
				x++;
			}
		}
		else if (player.x == x - 1 && player.y == y + 1)
		{
			if (currentRoom.CheckMovable(x, y + 1) == "Floor")
			{
				y++;
			}
			else if (currentRoom.CheckMovable(x - 1, y) == "Floor")
			{
				x--;
			}
		}
		else if (player.x == x + 1 && player.y == y + 1)
		{
			if (currentRoom.CheckMovable(x, y + 1) == "Floor")
			{
				y++;
			}
			else if (currentRoom.CheckMovable(x + 1, y) == "Floor")
			{
				x++;
			}
		}
		else if ((player.x == x &&(player.y == y + 1 || player.y - 1 == y)) || (player.y == y &&(player.x == x + 1 || player.x == x - 1)))
        {
			Attack(player);
			return true;
        }
		return false;
		
	}
    
	public void ModifyHealth(int modBy)
    {
		health += modBy;
    }
	
    
}

using System;

public class Player
{

	public String name;
	public int health;
	public int maxHealth;
	public int x;
	public int y;
	Random ranGen = new Random();
	public Player()
	{
		maxHealth = ranGen.Next(35, 55);
		health = maxHealth;
	}
	public int Attack()
    {
		
		return ranGen.Next(3, 7) * -1;
    }
	public void ModifyHealth(int modBy)
    {
		health += modBy;
		if (health > maxHealth)
		{
			health = maxHealth;
        }
    }
	public void RandomName()
    {
		string p1 = "";
		string p2 = "";
		string p3 = "";
		int p1rand = ranGen.Next(1, 3);
		int p2rand = ranGen.Next(1, 3);
		int p3rand = ranGen.Next(1, 3);
		if (p1rand == 1)
        {
			p1 = "Kal";
        }
		else if (p1rand == 2)
        {
			p1 = "Bar";
        }
		else if (p1rand == 3)
        {
			p1 = "San";
        }
		if (p2rand == 1)
		{
			p2 = "ar";
		}
		else if (p2rand == 2)
        {
			p2 = "il";
        }
		else if (p2rand == 3)
        {
			p2 = "gan";
        }
		if (p3rand == 1)
        {
			p3 = "Liran";
        }
		else if (p3rand == 2)
        {
			p3 = "Giral";
        }
		else if (p3rand == 3)
        {
			p3 = "Zaren";
        }
		name = p1 + p2 + " " + p3;
		
    }
	
}
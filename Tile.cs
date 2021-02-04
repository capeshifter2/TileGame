using System;

public class MapTile
{
	public enum TileType
	{
		Wall,
		Floor,
		Door
	}
	private TileType tileType;
	public bool isSeen;
	public MapTile()
	{
		tileType = TileType.Floor;
	}

	public MapTile(TileType tileType)
    {
		this.tileType = tileType;
    }

	public TileType getTileType()
    {
		return this.tileType;
    }

	public void setTileType(TileType tileType)
	{
		this.tileType = tileType;
	}
	public bool IsSeen()
    {
		return isSeen;
    }
}

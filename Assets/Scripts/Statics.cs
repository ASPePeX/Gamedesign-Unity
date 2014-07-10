using System;
using UnityEngine;

public static class Statics
{

    #region Basic level settings

    private const int _horizontalTiles = 40;
    private const int _verticalTiles = 20;
    private const int _tileSize = 32;
    private const int _actionPoints = 7;
	//added by konstantin
	private const int _healthPoints = 100;
	//

    public static int HorizontalTiles
    {
        get { return _horizontalTiles; }
    }

    public static int VerticalTiles
    {
        get { return _verticalTiles; }
    }

    public static int TileSize
    {
        get { return _tileSize; }
    }

    public static int ActionPoints
    {
        get { return _actionPoints; }
    }
	//added by konstantin
	public static int HealthPoints
	{
		get { return _healthPoints; }
	}
	//
    #endregion

    #region Color variables

    private static readonly Color _black = new Color(0, 0, 0, 1);
    private static readonly Color _gray = new Color(0, 0, 0, 0.6f);
    private static readonly Color _red = new Color(1, 0, 0, 0.3f);
    private static readonly Color _lightGreen = new Color(0, 1, 0, 0.1f);
    private static readonly Color _darkGreen = new Color(0, 1, 0, 0.3f);
    private static readonly Color _blue = new Color(0, 0, 1, 0.3f);
    private static readonly Color _yellow = new Color(1, 1, 0, 0.3f);
    private static readonly Color _semiTransparent = new Color(1, 1, 1, 0.3f);

    public static Color Black
    {
        get { return _black; }
    }

    public static Color Gray
    {
        get { return _gray; }
    }

    public static Color Red
    {
        get { return _red; }
    }

    public static Color LightGreen
    {
        get { return _lightGreen; }
    }

    public static Color DarkGreen
    {
        get { return _darkGreen; }
    }

    public static Color Blue
    {
        get { return _blue; }
    }

    public static Color Yellow
    {
        get { return _yellow; }
    }

    public static Color SemiTransparent
    {
        get { return _semiTransparent; }
    }

    #endregion

    #region Global convenience methods

    public static IntVector2 PosToTile(Vector3 pos)
    {
        return PosToTile(pos.x, pos.y);
    }
    
    public static IntVector2 PosToTile(float posX, float posY)
    {
        return PosToTile(new Vector2(posX, posY));
    }
    public static IntVector2 PosToTile(Vector2 pos)
    {
        return new IntVector2(Mathf.Clamp(Mathf.FloorToInt((pos.x + (TileSize / 100f / 2f * (HorizontalTiles - 1))) / (TileSize / 100f)), 0, HorizontalTiles - 1), Mathf.Clamp(Mathf.FloorToInt((pos.y + (TileSize / 100f / 2f * (VerticalTiles - 1))) / (TileSize / 100f)), 0, VerticalTiles - 1));
    }

    public static Vector2 TileToPos(int tileX, int tileY)
    {
        return new Vector2(tileX * (TileSize / 100f) - (TileSize / 100f / 2f * (HorizontalTiles - 1)), tileY * (TileSize / 100f) - (TileSize / 100f / 2f * (VerticalTiles - 1)));
    }
    public static Vector2 TileToPos(IntVector2 tile)
    {
        return new Vector2(tile.x * (TileSize / 100f) - (TileSize / 100f / 2f * (HorizontalTiles - 1)), tile.y * (TileSize / 100f) - (TileSize / 100f / 2f * (VerticalTiles - 1)));
    }

    public static int MovingToTileCost(GameObject go1, GameObject go2)
    {
        IntVector2 pos1 = new IntVector2();
        IntVector2 pos2 = new IntVector2();

        //Todo: Error handling in case of unsupported gameObjects
        if (go1.tag == "Player")
        {
            pos1 = go1.GetComponent<PlayerActions>().FinalPosition;
        }
        else if (go1.tag == "Enemy")
        {
            pos1 = PosToTile(go1.transform.position);
        }
        if (go2.tag == "Player")
        {
            pos2 = go2.GetComponent<PlayerActions>().FinalPosition;
        }
        else if (go2.tag == "Enemy")
        {
            pos2 = PosToTile(go2.transform.position);
        }

        return MovingToTileCost(pos1, pos2);
    }

    public static int MovingToTileCost(IntVector2 tileTargetPosition, GameObject player)
    {
        PlayerActions playerScript = (PlayerActions)player.GetComponent(typeof(PlayerActions));

        //Debug.Log(Math.Ceiling(Mathf.Sqrt(Mathf.Pow(playerScript.FinalPosition.x - tileTargetPosition.x, 2) + Mathf.Pow(playerScript.FinalPosition.y - tileTargetPosition.y, 2)) * 2));

        return (int)Math.Ceiling(Mathf.Sqrt(Mathf.Pow(playerScript.FinalPosition.x - tileTargetPosition.x, 2) + Mathf.Pow(playerScript.FinalPosition.y - tileTargetPosition.y, 2)) * 2);
    }

    public static int MovingToTileCost(IntVector2 position1, IntVector2 position2)
    {
        return (int)Math.Ceiling(Mathf.Sqrt(Mathf.Pow(position1.x - position2.x, 2) + Mathf.Pow(position1.y - position2.y, 2)) * 2);
    }

    #endregion

}

public struct IntVector2
{
    // ReSharper disable once InconsistentNaming
    public int x { get; set; }
    // ReSharper disable once InconsistentNaming
    public int y { get; set; }

    public IntVector2(int x, int y)
        : this()
    {
        this.x = x;
        this.y = y;
    }

    // ReSharper disable once UnusedMember.Local
    int SqrMagnitude
    {
        get { return x * x + y * y; }
    }

    public override string ToString()
    {
        return x + ", " + y;
    }

}

public struct ActionEntry
{
    private GameObject _goFrom;
    private GameObject _item;
    private GameObject _goTo;

    public ActionEntry(GameObject goFrom, GameObject item, GameObject goTo) : this()
    {
        GoFrom = goFrom;
        Item = item;
        GoTo = goTo;
    }

    public GameObject GoFrom
    {
        get { return _goFrom; }
        set { _goFrom = value; }
    }

    public GameObject Item
    {
        get { return _item; }
        set { _item = value; }
    }

    public GameObject GoTo
    {
        get { return _goTo; }
        set { _goTo = value; }
    }
}
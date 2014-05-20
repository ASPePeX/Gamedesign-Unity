using UnityEngine;

public static class Statics
{

    #region Basic level settings

    private const int _horizontalTiles = 40;
    private const int _verticalTiles = 20;
    private const int _tileSize = 32;
    private const int _actionPoints = 7;

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

    #endregion

    #region Color variables

    private static readonly Color _black = new Color(0, 0, 0, 1);
    private static readonly Color _gray = new Color(0, 0, 0, 0.6f);
    private static readonly Color _red = new Color(1, 0, 0, 0.3f);
    private static readonly Color _lightGreen = new Color(0, 1, 0, 0.2f);
    private static readonly Color _darkGreen = new Color(0, 1, 0, 0.5f);
    private static readonly Color _blue = new Color(0, 0, 1, 0.3f);
    private static readonly Color _yellow = new Color(1, 1, 0, 0.3f);

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

    #endregion

    #region Global convenience methods
    public static IntVector2 PosToTile(float posX, float posY)
    {
        return new IntVector2(Mathf.Clamp(Mathf.FloorToInt((posX + (TileSize / 100f / 2f * (HorizontalTiles - 1))) / (TileSize / 100f)), 0, HorizontalTiles - 1), Mathf.Clamp(Mathf.FloorToInt((posY + (TileSize / 100f / 2f * (VerticalTiles - 1))) / (TileSize / 100f)), 0, VerticalTiles - 1));
    }

    public static Vector2 TileToPos(int tileX, int tileY)
    {
        return new Vector2(tileX * (TileSize / 100f) - (TileSize / 100f / 2f * (HorizontalTiles - 1)), tileY * (TileSize / 100f) - (TileSize / 100f / 2f * (VerticalTiles - 1)));
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
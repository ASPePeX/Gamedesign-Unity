using UnityEngine;

public static class Statics {

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

    public static IntVector2 PosToTile(float posX, float posY)
    {
        return new IntVector2(Mathf.Clamp(Mathf.FloorToInt((posX + (TileSize / 100f / 2f * (HorizontalTiles - 1))) / (TileSize / 100f)), 0, HorizontalTiles - 1), Mathf.Clamp(Mathf.FloorToInt((posY + (TileSize / 100f / 2f * (VerticalTiles - 1))) / (TileSize / 100f)), 0, VerticalTiles - 1));
    }

    public static Vector2 TileToPos(int tileX, int tileY)
    {
        return new Vector2(tileX * (TileSize / 100f) - (TileSize / 100f / 2f * (HorizontalTiles - 1)), tileY * (TileSize / 100f) - (TileSize / 100f / 2f * (VerticalTiles - 1)));
    }

}

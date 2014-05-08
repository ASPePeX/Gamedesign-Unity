using UnityEngine;

public class BehaviorLevel : MonoBehaviour {

	private const int HorizontalTiles = 40;
	private const int VerticalTiles = 20;
	private const int TileSize = 32;
    private const int VisibilityRadius = 5;

    private bool[,] _isVisible;
    private bool[,] _wasVisible; 
    
    private GameObject[,] _overlayBlack;
    private GameObject[,] _overlayGray;

    private GameObject[,] _players;
    private GameObject[,] _items;
    private GameObject[,] _enemies;

    public GameObject TileOverlayGreen;
    public GameObject TileOverlayRed;
    public GameObject TileOverlayBlue;
    public GameObject TileOverlayYellow;
    public GameObject TileOverlayGray;
    public GameObject TileOverlayBlack;
    public GameObject TileOverlayHighlight;
    public GameObject TileOverlayTransparent;

    public Camera MainCamera;

    public Transform OverlayInstanciateTarget;
    public Transform PlayerSearchTarget;
    public Transform EnemySearchTarget;
    public Transform ItemSearchTarget;

    //test variables
    public GameObject ActivePlayer;

// ReSharper disable once UnusedMember.Local
    public void Start ()
    {
        _overlayBlack = new GameObject[HorizontalTiles, VerticalTiles];
        _overlayGray = new GameObject[HorizontalTiles, VerticalTiles];
        _players = new GameObject[HorizontalTiles, VerticalTiles];
        _enemies = new GameObject[HorizontalTiles, VerticalTiles];
        _items = new GameObject[HorizontalTiles, VerticalTiles];

        _isVisible = new bool[HorizontalTiles, VerticalTiles];
        _wasVisible = new bool[HorizontalTiles, VerticalTiles];

	    for (int i = 0; i < HorizontalTiles; i++)
	    {
	        for (int j = 0; j < VerticalTiles; j++)
	        {
	            Vector2 posXY = TileToPos(i, j);

                _overlayBlack[i, j] = Instantiate(TileOverlayBlack, new Vector3(posXY.x, posXY.y, 0), Quaternion.identity) as GameObject;
                _overlayBlack[i, j].transform.parent = OverlayInstanciateTarget;
                _overlayGray[i, j] = Instantiate(TileOverlayGray, new Vector3(posXY.x, posXY.y, 0), Quaternion.identity) as GameObject;
                _overlayGray[i, j].transform.parent = OverlayInstanciateTarget;
            }
	    }

        for (int i = 0; i < PlayerSearchTarget.childCount; i++)
        {
            GameObject player = PlayerSearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = PosToTile(player.transform.position.x, player.transform.position.y);

            Debug.Log("Player " + i + ": " + tileXY);

            _players[tileXY.x, tileXY.x] = player;
        }

        for (int i = 0; i < EnemySearchTarget.childCount; i++)
        {
            GameObject enemy = EnemySearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = PosToTile(enemy.transform.position.x, enemy.transform.position.y);

            Debug.Log("Enemy " + i + ": " + tileXY);

            _enemies[tileXY.x, tileXY.x] = enemy;
        }

        for (int i = 0; i < ItemSearchTarget.childCount; i++)
        {
            GameObject item = ItemSearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = PosToTile(item.transform.position.x, item.transform.position.y);

            Debug.Log("Item " + i + ": " + tileXY);

            _items[tileXY.x, tileXY.x] = item;
        }

        ActivePlayer = PlayerSearchTarget.GetChild(0).gameObject;

        EvaluateTileOverlayAndVisibility();

    }
	
// ReSharper disable once UnusedMember.Local
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        Vector2 mouseWorldPos = MainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
	        IntVector2 toTileXY = PosToTile(mouseWorldPos.x + TileSize/100f/2f, mouseWorldPos.y + TileSize/100f/2f);
            IntVector2 fromTileXY = PosToTile(ActivePlayer.transform.position.x, ActivePlayer.transform.position.y);

	        _players[toTileXY.x, toTileXY.y] = ActivePlayer;
	        _players[fromTileXY.x, fromTileXY.y] = null;

	        Vector2 toPosXY = TileToPos(toTileXY.x, toTileXY.y);
	        ActivePlayer.transform.position = new Vector3(toPosXY.x, toPosXY.y, 0);

            EvaluateTileOverlayAndVisibility();

	    }
	}

    public void EvaluateTileOverlayAndVisibility()
    {

        //reseting visibility
        for (int j = 0; j < HorizontalTiles; j++)
        {
            for (int k = 0; k < VerticalTiles; k++)
            {
                _isVisible[j, k] = false;
            }
        }

        //Evaluating visibility for each player and adding it up
        for (int i = 0; i < PlayerSearchTarget.childCount; i++)
        {
            GameObject player = PlayerSearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = PosToTile(player.transform.position.x, player.transform.position.y);

            var isVisibleToPlayer = new bool[HorizontalTiles, VerticalTiles];

            for (int j = 0; j < HorizontalTiles; j++)
            {
                for (int k = 0; k < VerticalTiles; k++)
                {
                    if (Mathf.Sqrt(Mathf.Pow(j - tileXY.x, 2) + Mathf.Pow(k - tileXY.y, 2)) < VisibilityRadius)
                    {
                        Debug.Log(j + " " + k +" " +Mathf.Sqrt(Mathf.Pow(j - tileXY.x, 2) + Mathf.Pow(k - tileXY.y, 2)));
                        isVisibleToPlayer[j, k] = true;
                    }
                }
            }

            for (int j = 0; j < HorizontalTiles; j++)
            {
                for (int k = 0; k < VerticalTiles; k++)
                {
                    if (isVisibleToPlayer[j, k])
                    {
                        _isVisible[j, k] = true;
                        _wasVisible[j, k] = true;
                    }
                }
            }
        }

        //Overlay GameObjects are set active according to visibility
        for (int j = 0; j < HorizontalTiles; j++)
        {
            for (int k = 0; k < VerticalTiles; k++)
            {
                if (_isVisible[j, k])
                {
                    _overlayBlack[j, k].SetActive(false);
                    _overlayGray[j, k].SetActive(false);
                }
                else if (_wasVisible[j,k])
                {
                    _overlayBlack[j, k].SetActive(false);
                    _overlayGray[j, k].SetActive(true);
                }
                else
                {
                    _overlayGray[j, k].SetActive(true);
                }
            }
        }

        //Enemy GameObjects are set active according to visibility
        for (int i = 0; i < EnemySearchTarget.childCount; i++)
        {
            GameObject enemy = EnemySearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = PosToTile(enemy.transform.position.x, enemy.transform.position.y);

            enemy.SetActive(_isVisible[tileXY.x, tileXY.y]);
        }

        //Item GameObjects are set active according to visibility
        for (int i = 0; i < ItemSearchTarget.childCount; i++)
        {
            GameObject item = ItemSearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = PosToTile(item.transform.position.x, item.transform.position.y);

            item.SetActive(_isVisible[tileXY.x, tileXY.y]);
        }
    }

    public IntVector2 PosToTile(float posX, float posY)
    {
        return new IntVector2(Mathf.Clamp(Mathf.FloorToInt((posX + (TileSize / 100f / 2f * (HorizontalTiles - 1))) / (TileSize / 100f)), 0, HorizontalTiles-1), Mathf.Clamp(Mathf.FloorToInt((posY + (TileSize / 100f / 2f * (VerticalTiles - 1))) / (TileSize / 100f)), 0, VerticalTiles-1));
    }

    public Vector2 TileToPos(int tileX, int tileY)
    {
        return new Vector2(tileX * (TileSize / 100f) - (TileSize / 100f / 2f * (HorizontalTiles - 1)), tileY * (TileSize / 100f) - (TileSize / 100f / 2f * (VerticalTiles - 1)));
    }
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
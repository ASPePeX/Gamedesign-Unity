using System;
using UnityEngine;

public class BehaviorLevel : MonoBehaviour {

    private const int VisibilityRadius = 5;

    private bool[,] _isVisible;
    private bool[,] _wasVisible;

    private bool[,] _isTraversable;
    
    private GameObject[,] _overlay;

    private GameObject[,] _players;
    private GameObject[,] _items;
    private GameObject[,] _enemies;

    public GameObject TileOverlay;
    public GameObject TileOverlayHighlight;

    private readonly Color _black =         new Color(0, 0, 0, 1);
    private readonly Color _gray =          new Color(0, 0, 0, 0.6f);
    private readonly Color _red =           new Color(1, 0, 0, 0.3f);
    private readonly Color _lightGreen =    new Color(0, 1, 0, 0.2f);
    private readonly Color _darkGreen =     new Color(0, 1, 0, 0.5f);
    private readonly Color _blue =          new Color(0, 0, 1, 0.3f);
    private readonly Color _yellow =        new Color(1, 1, 0, 0.3f);

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
        _isTraversable = new bool[,]
        {
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, false, false, false, false, false, false, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, false, false, false, false, false, false, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, false, false, false, false, false, false, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, false, false, false, false, false, false, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, false, false, true, true, false, false, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true}
        };

        TileOverlay.GetComponent<SpriteRenderer>().color = _black;

        _overlay = new GameObject[Statics.HorizontalTiles, Statics.VerticalTiles];
        _players = new GameObject[Statics.HorizontalTiles, Statics.VerticalTiles];
        _enemies = new GameObject[Statics.HorizontalTiles, Statics.VerticalTiles];
        _items = new GameObject[Statics.HorizontalTiles, Statics.VerticalTiles];

        _isVisible = new bool[Statics.HorizontalTiles, Statics.VerticalTiles];
        _wasVisible = new bool[Statics.HorizontalTiles, Statics.VerticalTiles];

        for (int i = 0; i < Statics.HorizontalTiles; i++)
	    {
            for (int j = 0; j < Statics.VerticalTiles; j++)
	        {
                Vector2 posXY = Statics.TileToPos(i, j);

                _overlay[i, j] = Instantiate(TileOverlay, new Vector3(posXY.x, posXY.y, 0), Quaternion.identity) as GameObject;
                _overlay[i, j].transform.parent = OverlayInstanciateTarget;
	            _overlay[i, j].name = "Tile-" + j + "x" + i;
	        }
	    }

        for (int i = 0; i < PlayerSearchTarget.childCount; i++)
        {
            GameObject player = PlayerSearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = Statics.PosToTile(player.transform.position.x, player.transform.position.y);

            Debug.Log("Player " + i + ": " + tileXY);

            _players[tileXY.x, tileXY.y] = player;
        }

        for (int i = 0; i < EnemySearchTarget.childCount; i++)
        {
            GameObject enemy = EnemySearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = Statics.PosToTile(enemy.transform.position.x, enemy.transform.position.y);

            Debug.Log("Enemy " + i + ": " + tileXY);

            _enemies[tileXY.x, tileXY.y] = enemy;
        }

        for (int i = 0; i < ItemSearchTarget.childCount; i++)
        {
            GameObject item = ItemSearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = Statics.PosToTile(item.transform.position.x, item.transform.position.y);

            Debug.Log("Item " + i + ": " + tileXY);

            _items[tileXY.x, tileXY.y] = item;
        }

        ActivePlayer = PlayerSearchTarget.GetChild(0).gameObject;

        EvaluateTileOverlayAndVisibility();

    }
	
// ReSharper disable once UnusedMember.Local
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        Vector2 mouseWorldPos = MainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            IntVector2 toTileXY = Statics.PosToTile(mouseWorldPos.x + Statics.TileSize / 100f / 2f, mouseWorldPos.y + Statics.TileSize / 100f / 2f);
            IntVector2 fromTileXY = Statics.PosToTile(ActivePlayer.transform.position.x, ActivePlayer.transform.position.y);

	        if (_isTraversable[toTileXY.x, toTileXY.y] && _enemies[toTileXY.x, toTileXY.y] == null && _items[toTileXY.x, toTileXY.y] == null)
	        {
	            _players[toTileXY.x, toTileXY.y] = ActivePlayer;
	            _players[fromTileXY.x, fromTileXY.y] = null;

	            Vector2 toPosXY = Statics.TileToPos(toTileXY.x, toTileXY.y);
	            ActivePlayer.transform.position = new Vector3(toPosXY.x, toPosXY.y, 0);

	            EvaluateTileOverlayAndVisibility();
	        }
	    }
	}

    public void EvaluateTileOverlayAndVisibility()
    {

        //reseting visibility
        for (int j = 0; j < Statics.HorizontalTiles; j++)
        {
            for (int k = 0; k < Statics.VerticalTiles; k++)
            {
                _isVisible[j, k] = false;
            }
        }

        //Evaluating visibility for each player and adding it up
        for (int i = 0; i < PlayerSearchTarget.childCount; i++)
        {
            GameObject player = PlayerSearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = Statics.PosToTile(player.transform.position.x, player.transform.position.y);

            var isVisibleToPlayer = new bool[Statics.HorizontalTiles, Statics.VerticalTiles];

            for (int j = 0; j < Statics.HorizontalTiles; j++)
            {
                for (int k = 0; k < Statics.VerticalTiles; k++)
                {
                    if (Mathf.Sqrt(Mathf.Pow(j - tileXY.x, 2) + Mathf.Pow(k - tileXY.y, 2)) < VisibilityRadius)
                    {
                        //Debug.Log(j + " " + k +" " +Mathf.Sqrt(Mathf.Pow(j - tileXY.x, 2) + Mathf.Pow(k - tileXY.y, 2)));
                        isVisibleToPlayer[j, k] = true;
                    }
                }
            }

            for (int j = 0; j < Statics.HorizontalTiles; j++)
            {
                for (int k = 0; k < Statics.VerticalTiles; k++)
                {
                    if (isVisibleToPlayer[j, k])
                    {
                        _isVisible[j, k] = true;
                        _wasVisible[j, k] = true;
                    }
                }
            }
        }

        //Evaluate visibility to tile color
        for (int j = 0; j < Statics.HorizontalTiles; j++)
        {
            for (int k = 0; k < Statics.VerticalTiles; k++)
            {
                if (_isVisible[j, k])
                {
                    _overlay[j, k].SetActive(false);
                }
                else if (_wasVisible[j,k])
                {
                    _overlay[j, k].SetActive(true);
                    _overlay[j, k].GetComponent<SpriteRenderer>().color = _gray;
                }
            }
        }

        //Enemy GameObjects are set active according to visibility
        for (int i = 0; i < EnemySearchTarget.childCount; i++)
        {
            GameObject enemy = EnemySearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = Statics.PosToTile(enemy.transform.position.x, enemy.transform.position.y);

            enemy.SetActive(_isVisible[tileXY.x, tileXY.y]);
        }

        //Item GameObjects are set active according to visibility
        for (int i = 0; i < ItemSearchTarget.childCount; i++)
        {
            GameObject item = ItemSearchTarget.GetChild(i).gameObject;

            IntVector2 tileXY = Statics.PosToTile(item.transform.position.x, item.transform.position.y);

            item.SetActive(_isVisible[tileXY.x, tileXY.y]);
        }
    }

    public void DrawMovement(string positionString)
    {
        EvaluateTileOverlayAndVisibility();

        //Todo: Input validation!
        string[] msg = positionString.Split('x');
        IntVector2 centerPosition = new IntVector2(Convert.ToInt32(msg[0]), Convert.ToInt32(msg[1]));
        int actionPoints = Convert.ToInt32(msg[2]);

        //doesn't cover reset for overlapping visibility Todo: make better
        for (int j = 0; j < Statics.HorizontalTiles; j++)
        {
            for (int k = 0; k < Statics.VerticalTiles; k++)
            {
                //Debug.Log(Mathf.Sqrt(Mathf.Pow(j - centerPosition.x, 2) + Mathf.Pow(k - centerPosition.y, 2)) * 2);
                if (Mathf.Sqrt(Mathf.Pow(j - centerPosition.x, 2) + Mathf.Pow(k - centerPosition.y, 2))*2 < actionPoints)
                {
                    if (_isTraversable[j, k])
                    {
                        if (_items[j, k] == null && _enemies[j, k] == null)
                        {
                            _overlay[j, k].GetComponent<SpriteRenderer>().color = _lightGreen;
                            _overlay[j, k].SetActive(true);
                        }
                        else if (_items[j, k])
                        {
                            _overlay[j, k].GetComponent<SpriteRenderer>().color = _yellow;
                            _overlay[j, k].SetActive(true);
                        }
                        else if (_enemies[j, k])
                        {
                            _overlay[j, k].GetComponent<SpriteRenderer>().color = _red;
                            _overlay[j, k].SetActive(true);
                        }
                    }

                }
            }
        }

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
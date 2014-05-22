using UnityEngine;

public class BehaviorLevel : MonoBehaviour {

    private const int VisibilityRadius = 5;

    private bool[,] _isVisible;
    private bool[,] _wasVisible;

    private bool[,] _isTraversable;
    
    private GameObject[,] _overlay;

    private GameObject[] _players;
    private GameObject[,] _items;
    private GameObject[,] _enemies;
    private GameObject _activePlayer;

    public GameObject TileOverlay;
    public GameObject TileOverlayHighlight;
    public GameObject PlayerGhost;

    public Camera MainCamera;

    public Transform OverlayInstanciateTarget;
    public Transform InstanciateTarget;
    public Transform PlayerSearchTarget;
    public Transform EnemySearchTarget;
    public Transform ItemSearchTarget;

// ReSharper disable once UnusedMember.Local
    public void Start ()
    {
        #region _istraversable
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
        #endregion

        TileOverlay.GetComponent<SpriteRenderer>().color = Statics.Black;

        _overlay = new GameObject[Statics.HorizontalTiles, Statics.VerticalTiles];
        _players = new GameObject[PlayerSearchTarget.childCount];
        _enemies = new GameObject[Statics.HorizontalTiles, Statics.VerticalTiles];
        _items = new GameObject[Statics.HorizontalTiles, Statics.VerticalTiles];
//        _activePlayer = new GameObject();

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
            _players[i] = PlayerSearchTarget.GetChild(i).gameObject;
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

        EvaluateTileOverlayAndVisibility();

    }
	
// ReSharper disable once UnusedMember.Local
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        Vector2 mouseWorldPos = MainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            IntVector2 clickTilePosition = Statics.PosToTile(mouseWorldPos.x + Statics.TileSize / 100f / 2f, mouseWorldPos.y + Statics.TileSize / 100f / 2f);

            //If no player is active, select an active player
	        if (_activePlayer == null)
	        {
	            for (int i = 0; i < _players.Length; i++)
	            {
	                PlayerActions currentPlayerScript = (PlayerActions) _players[i].GetComponent(typeof (PlayerActions));

                    if (currentPlayerScript.StartPosition.Equals(clickTilePosition) && !currentPlayerScript.RoundFinished)
                    {
                        _activePlayer = _players[i];
                        currentPlayerScript.Active = true;
                        DrawMovement(currentPlayerScript.StartPosition, currentPlayerScript.ActionPoints);
                    }
	            }
	        }
            //If we have an active player ...
	        else
	        {
	            PlayerActions activePlayerScript = (PlayerActions) _activePlayer.GetComponent(typeof (PlayerActions));

                //if we click on the player
	            if (Statics.PosToTile(_activePlayer.transform.position.x, _activePlayer.transform.position.y).Equals(clickTilePosition))
	            {
	                activePlayerScript.ClearGhosts();
                    DrawMovement(clickTilePosition, activePlayerScript.ActionPoints);
	            }
                
                //if we click on an active players ghost
                else if (activePlayerScript.CheckIfGhost(clickTilePosition))
                {
                    IntVector2 lastGhostPosition = activePlayerScript.RemoveGhosts(clickTilePosition);
                    DrawMovement(lastGhostPosition, activePlayerScript.ActionPoints);
                }

                //if we click on an empty tile a player can move to depending on their current action points
                else if (Statics.MovingToTileCost(clickTilePosition, _activePlayer) <= activePlayerScript.ActionPoints && Statics.MovingToTileCost(clickTilePosition, _activePlayer) > 1 && _isTraversable[clickTilePosition.x, clickTilePosition.y] && _items[clickTilePosition.x, clickTilePosition.y] == null && _enemies[clickTilePosition.x, clickTilePosition.y] == null)
	            {
                    activePlayerScript.AddGhost(clickTilePosition);

                    activePlayerScript.ActionPoints -= Statics.MovingToTileCost(clickTilePosition, _activePlayer);
                    activePlayerScript.FinalPosition = clickTilePosition;

                    DrawMovement(clickTilePosition, activePlayerScript.ActionPoints);
                }
	        }


	    }

	    if (Input.GetKeyDown(KeyCode.P) && _activePlayer != null)
	    {
            ClearMovement();
            PlayerActions activePlayerScript = (PlayerActions)_activePlayer.GetComponent(typeof(PlayerActions));
	        activePlayerScript.RoundFinished = true;
	        _activePlayer = null;

            //this is set and evaluated by the following for loop
            bool isRoundDoneFlag = true;

            for (int i = 0; i < _players.Length; i++)
	        {
                PlayerActions playerScript = (PlayerActions)_players[i].GetComponent(typeof(PlayerActions));
	            if (!playerScript.RoundFinished)
	            {
	                isRoundDoneFlag = false;
	            }
	        }

	        if (isRoundDoneFlag)
	        {
	            BroadcastMessage("MessageHandler", "NewRound");
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
                    _overlay[j, k].GetComponent<SpriteRenderer>().color = Statics.Gray;
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

    public void DrawMovement(IntVector2 centerPosition, int actionPoints)
    {



        for (int j = 0; j < Statics.HorizontalTiles; j++)
        {
            for (int k = 0; k < Statics.VerticalTiles; k++)
            {
                //Reset previous movement draw
                if (_overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.LightGreen || _overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.Yellow || _overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.Red)
                    _overlay[j, k].SetActive(false);

                //Debug.Log(Mathf.Sqrt(Mathf.Pow(j - centerPosition.x, 2) + Mathf.Pow(k - centerPosition.y, 2)) * 2);
                if (Mathf.Sqrt(Mathf.Pow(j - centerPosition.x, 2) + Mathf.Pow(k - centerPosition.y, 2))*2 < actionPoints)
                {
                    if (_isTraversable[j, k])
                    {
                        if (_items[j, k] == null && _enemies[j, k] == null)
                        {
                            _overlay[j, k].GetComponent<SpriteRenderer>().color = Statics.LightGreen;
                            _overlay[j, k].SetActive(true);
                        }
                        else if (_items[j, k])
                        {
                            _overlay[j, k].GetComponent<SpriteRenderer>().color = Statics.Yellow;
                            _overlay[j, k].SetActive(true);
                        }
                        else if (_enemies[j, k])
                        {
                            _overlay[j, k].GetComponent<SpriteRenderer>().color = Statics.Red;
                            _overlay[j, k].SetActive(true);
                        }
                    }

                }
            }
        }

    }

    public void ClearMovement()
    {
        for (int j = 0; j < Statics.HorizontalTiles; j++)
        {
            for (int k = 0; k < Statics.VerticalTiles; k++)
            {
                //Reset previous movement draw
                if (_overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.LightGreen || _overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.Yellow || _overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.Red)
                    _overlay[j, k].SetActive(false);
            }
        }
    }



}

﻿using System;
using System.Text;
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

    private GameObject[] _availableItems;

    private GameObject _dropItem;

    private bool refresh;

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

        _availableItems = GetComponent<AvailableItems>()._items;

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
	    if (Input.GetKeyDown(KeyCode.I))
	    {
	        StartDropItem(_availableItems[0].name);
	    }

	    if (Input.GetMouseButtonDown(0) || refresh)
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
                        DrawMovement(currentPlayerScript.StartPosition, currentPlayerScript.ActionPoints, currentPlayerScript.LastAction);
                    }
	            }
	        }
            //If we have an active player ...
	        else
	        {
                Debug.Log(CheckIfPlayerOnTile(clickTilePosition));
	            PlayerActions activePlayerScript = (PlayerActions) _activePlayer.GetComponent(typeof (PlayerActions));

                //if the gui sent us an item to drop
                if (_dropItem != null && refresh && activePlayerScript.ActionPoints > 1)
	            {
	                DrawDropArea(activePlayerScript.FinalPosition, activePlayerScript.ActionPoints);
	            }

                //if we click on the player
	            else if (Statics.PosToTile(_activePlayer.transform.position.x, _activePlayer.transform.position.y).Equals(clickTilePosition))
	            {
                    _dropItem = null;
                    activePlayerScript.ClearGhosts();
                    
	                DrawMovement(clickTilePosition, activePlayerScript.ActionPoints, activePlayerScript.LastAction);
	            }

                //if we click on an active players ghost
                else if (activePlayerScript.CheckIfPlayerGhost(clickTilePosition))
                {
                    _dropItem = null;
                    IntVector2 lastGhostPosition = activePlayerScript.RemovePlayerGhosts(clickTilePosition);
                    DrawMovement(lastGhostPosition, activePlayerScript.ActionPoints, activePlayerScript.LastAction);
                }

                //if we have an item from the gui and click somewhere
                else if (_dropItem != null && !refresh && activePlayerScript.ActionPoints > 1 && Statics.MovingToTileCost(clickTilePosition, _activePlayer) <= 3 && _items[clickTilePosition.x, clickTilePosition.y] == null && _isTraversable[clickTilePosition.x, clickTilePosition.y])
                {
                    _items[clickTilePosition.x, clickTilePosition.y] = Instantiate(_dropItem) as GameObject;
                    _items[clickTilePosition.x, clickTilePosition.y].transform.parent = ItemSearchTarget;
                    _items[clickTilePosition.x, clickTilePosition.y].name = _dropItem.name;
                    _items[clickTilePosition.x, clickTilePosition.y].transform.position = Statics.TileToPos(clickTilePosition.x, clickTilePosition.y);
                    activePlayerScript.ActionPoints -= 1;
                    //ToDo: Tell inventory that item has been dropped
                    Debug.Log(_activePlayer.name + " dropped " + _dropItem.name);

                    _dropItem = null;
                    DrawMovement(activePlayerScript.FinalPosition, activePlayerScript.ActionPoints, activePlayerScript.LastAction);
                }

                //if we click on an empty tile a player can move to depending on their current action points
                else if (!activePlayerScript.LastAction && Statics.MovingToTileCost(clickTilePosition, _activePlayer) <= activePlayerScript.ActionPoints && Statics.MovingToTileCost(clickTilePosition, _activePlayer) > 1 && _isTraversable[clickTilePosition.x, clickTilePosition.y] && _items[clickTilePosition.x, clickTilePosition.y] == null && _enemies[clickTilePosition.x, clickTilePosition.y] == null && _dropItem == null && !CheckIfPlayerOnTile(clickTilePosition))
	            {
                    activePlayerScript.AddPlayerGhost(clickTilePosition);

                    activePlayerScript.ActionPoints -= Statics.MovingToTileCost(clickTilePosition, _activePlayer);
                    activePlayerScript.FinalPosition = clickTilePosition;

                    DrawMovement(clickTilePosition, activePlayerScript.ActionPoints, activePlayerScript.LastAction);
                }
                //if we click on an active players ghosted item
                else if (activePlayerScript.CheckIfItemGhost(clickTilePosition) && _dropItem == null)
                {
                    activePlayerScript.RemoveItemGhosts(clickTilePosition);
                    activePlayerScript.ActionPoints += 1;

                    _items[clickTilePosition.x, clickTilePosition.y].SetActive(true);

                    DrawMovement(activePlayerScript.FinalPosition, activePlayerScript.ActionPoints, activePlayerScript.LastAction);
                }
                //if we click on an item in range
                    //ToDo: Check if there is space in the players inventory
                else if (Statics.MovingToTileCost(clickTilePosition, _activePlayer) <= 3 && activePlayerScript.ActionPoints > 1 && _items[clickTilePosition.x, clickTilePosition.y] != null && _items[clickTilePosition.x, clickTilePosition.y].activeSelf && _dropItem == null)
                {
                    activePlayerScript.AddItemGhost(clickTilePosition, _items[clickTilePosition.x, clickTilePosition.y].name);

                    activePlayerScript.ActionPoints -= 1;

                    _items[clickTilePosition.x, clickTilePosition.y].SetActive(false);

                    DrawMovement(clickTilePosition, activePlayerScript.ActionPoints, activePlayerScript.LastAction);
                }

                else
                {
                    _dropItem = null;
                    DrawMovement(activePlayerScript.FinalPosition, activePlayerScript.ActionPoints, activePlayerScript.LastAction);
                }
	        }
            refresh = false;
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
                //next round
                BroadcastMessage("MessageHandler", "NewRound");

                //clearing out inactive items
                for (int i = 0; i < Statics.HorizontalTiles; i++)
                {
                    for (int j = 0; j < Statics.VerticalTiles; j++)
                    {
                        if (_items[i, j] != null && !_items[i, j].activeSelf)
                        {
                            Destroy(_items[i, j]);
                            _items[i, j] = null;
                        }
                    }
                }
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

    public void DrawMovement(IntVector2 centerPosition, int actionPoints, bool movementOverride)
    {

        if (movementOverride)
            actionPoints = 0;

        for (int j = 0; j < Statics.HorizontalTiles; j++)
        {
            for (int k = 0; k < Statics.VerticalTiles; k++)
            {
                //Reset previous draw
                if (_overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.LightGreen || _overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.Yellow || _overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.Red)
                    _overlay[j, k].SetActive(false);

                //Debug.Log(Mathf.Sqrt(Mathf.Pow(j - centerPosition.x, 2) + Mathf.Pow(k - centerPosition.y, 2)) * 2);
                if (Mathf.Sqrt(Mathf.Pow(j - centerPosition.x, 2) + Mathf.Pow(k - centerPosition.y, 2))*2 < actionPoints)
                {
                    if (_isTraversable[j, k])
                    {
                        if (actionPoints > 1 && _items[j, k] == null && _enemies[j, k] == null)
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

    public void DrawDropArea(IntVector2 centerPosition, int actionPoints)
    {
        for (int j = 0; j < Statics.HorizontalTiles; j++)
        {
            for (int k = 0; k < Statics.VerticalTiles; k++)
            {
                //Reset previous draw
                if (_overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.LightGreen || _overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.Yellow || _overlay[j, k].GetComponent<SpriteRenderer>().color == Statics.Red)
                    _overlay[j, k].SetActive(false);

                //Debug.Log(Mathf.Sqrt(Mathf.Pow(j - centerPosition.x, 2) + Mathf.Pow(k - centerPosition.y, 2)) * 2);
                if (Mathf.Sqrt(Mathf.Pow(j - centerPosition.x, 2) + Mathf.Pow(k - centerPosition.y, 2))*2 < 3)
                {
                    if (_isTraversable[j, k])
                    {
                        _overlay[j, k].GetComponent<SpriteRenderer>().color = Statics.Yellow;
                        _overlay[j, k].SetActive(true);
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

    public void StartDropItem(String itemName)
    {
        for (int i = 0; i < _availableItems.Length; i++)
        {
            if (_availableItems[i].name == itemName)
            {
                _dropItem = _availableItems[i];
            }
        }
        refresh = true;
    }

    // Returns true if a player, a player ghost or an item ghost is on a tile
    private bool CheckIfPlayerOnTile(IntVector2 tilePosition)
    {
        foreach (GameObject player in _players)
        {
            PlayerActions playerScript = player.GetComponent<PlayerActions>();

            if (Statics.PosToTile(player.transform.position.x, player.transform.position.y).Equals(tilePosition) || playerScript.CheckIfPlayerGhost(tilePosition) || playerScript.CheckIfItemGhost(tilePosition))
            {
                return true;
            }
        }
        return false;
    }
}

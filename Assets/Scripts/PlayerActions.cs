using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject PlayerGhost;
    public Color playerColor;

    private bool _active;
    private bool _roundFinished;

    private bool _lastAction;

    private int _actionPoints;
	//added by konstantin
	private int _healthPoints;
	//
    private bool _isInfected;

    private IntVector2 _startPosition;
    private IntVector2 _finalPosition;
    private List<GameObject> _ghosts;
    private GameObject _ghostItem;

    public List<GameObject> _items;

    private int _activeItem;
    private int _primaryWeapon;
    private int _activeArmor;
    
    private GameObject _level;
    private GameObject[] _availableItems;

    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }

    public bool RoundFinished
    {
        get { return _roundFinished; }
        set { _roundFinished = value; }
    }

    public IntVector2 StartPosition
    {
        get { return _startPosition; }
        set { _startPosition = value; }
    }

    public IntVector2 FinalPosition
    {
        get { return _finalPosition; }
        set { _finalPosition = value; }
    }

    public int ActionPoints
    {
        get { return _actionPoints; }
        set { _actionPoints = value; }
    }
	//added by konstantin
	public int HealthPoints
	{
		get { return _healthPoints; }
		set { _healthPoints = value; }
	}
	//
    public bool LastAction
    {
        get { return _lastAction; }
        set { _lastAction = value; }
    }

    public List<GameObject> Items
    {
        get { return _items; }
        set { _items = value; }
    }
	//Added by Konstantin
	public GameObject GhostItem
	{
		get { return _ghostItem; }
		set { _ghostItem = value; }
	}
	//
    public int ActiveItem
    {
        get { return _activeItem; }
        set { _activeItem = value; }
    }

    public int PrimaryWeapon
    {
        get { return _primaryWeapon; }
        set { _primaryWeapon = value; }
    }

    public bool IsInfected
    {
        get { return _isInfected; }
        set { _isInfected = value; }
    }

    public int ActiveArmor
    {
        get { return _activeArmor; }
        set { _activeArmor = value; }
    }

    #region PlayerGhosts
    public void AddPlayerGhost(IntVector2 instantiatePosition)
    {
        Vector2 posXY = Statics.TileToPos(instantiatePosition.x, instantiatePosition.y);

        var newGhost = Instantiate(this.PlayerGhost, new Vector3(posXY.x, posXY.y, 0), Quaternion.identity) as GameObject;
        newGhost.transform.parent = this.transform;
        //newGhost.GetComponent<SpriteRenderer>().color = playerColor;
        _ghosts.Add(newGhost);
    }

    public bool CheckIfPlayerGhost(IntVector2 checkPosition)
    {
        for (int i = 0; i < _ghosts.Count; i++)
        {
            if (Statics.PosToTile(_ghosts[i].transform.position.x, _ghosts[i].transform.position.y).Equals(checkPosition))
            {
                return true;
            }
        }
        return false;
    }

    public IntVector2 RemovePlayerGhosts(IntVector2 removePosition)
    {
        bool found = false;
        for (int i = _ghosts.Count - 1; i > -1; i--)
        {
            if (Statics.PosToTile(_ghosts[i].transform.position.x, _ghosts[i].transform.position.y).Equals(removePosition))
            {
                FinalPosition = removePosition;
                break;
            }
            else
            {
                Destroy(_ghosts[i]);
                _ghosts.RemoveAt(i); 
            }
        }

        int newActionpoints = Statics.ActionPoints;
        IntVector2 lastPosition = StartPosition;

        foreach (GameObject ghost in _ghosts)
        {
            IntVector2 thisPosition = Statics.PosToTile(ghost.transform.position);
            newActionpoints -= Statics.MovingToTileCost(lastPosition, thisPosition);
            lastPosition = thisPosition;
        }
        ActionPoints = newActionpoints;

        return FinalPosition;
    }

    #endregion

    #region ItemGhosts
    public void AddItemGhost(IntVector2 instantiatePosition, String itemtype="")
    {
        GameObject instantiateItem = null;

        for (int i = 0; i < _availableItems.Length; i++)
        {
            if (_availableItems[i].name == itemtype)
            {
                instantiateItem = _availableItems[i];
            }
        }

        if (instantiateItem != null)
        {
            Vector2 posXY = Statics.TileToPos(instantiatePosition.x, instantiatePosition.y);

            var newGhost = Instantiate(instantiateItem, new Vector3(posXY.x, posXY.y, 0), Quaternion.identity) as GameObject;
            newGhost.transform.parent = this.transform;
            newGhost.name = itemtype;
            newGhost.GetComponent<SpriteRenderer>().color = Statics.SemiTransparent;
            _ghostItem = newGhost;

            LastAction = true;
        }
        else
        {
            Debug.LogError("Item ghost could not be instanciated!");
        }
    }

    public bool CheckIfItemGhost(IntVector2 checkPosition)
    {
        if (_ghostItem != null && Statics.PosToTile(_ghostItem.transform.position.x, _ghostItem.transform.position.y).Equals(checkPosition))
        {
            return true;
        }
        return false;
    }

    public void RemoveItemGhosts(IntVector2 removePosition)
    {
        if (Statics.PosToTile(_ghostItem.transform.position.x, _ghostItem.transform.position.y).Equals(removePosition))
        {
            Destroy(_ghostItem);
            _ghostItem = null;
        }

        LastAction = false;
    }

    #endregion
    public void ClearGhosts()
    {
        RoundStart();
    }

    // Use this for initialization
	void Start ()
	{
	    _level = GameObject.Find("/Level");
        _availableItems = _level.GetComponent<AvailableItems>()._items;
        _ghosts = new List<GameObject>();
		//_items = new List<GameObject>(); will be filled from the inspector with prefabs; only Level2.unity
        _ghostItem = null;
	    LastAction = false;

	    GetComponent<SpriteRenderer>().color = playerColor;
        HealthPoints = Statics.HealthPoints -50;

        RoundStart();
	}

    //called by the rounds manager (todo)
    public void RoundStart()
    {
        if (RoundFinished)
        {
            StartPosition = FinalPosition;
        }
        else
        {
            FinalPosition = Statics.PosToTile(this.transform.position.x, this.transform.position.y);
            StartPosition = FinalPosition;
        }

        Vector2 worldPosition = Statics.TileToPos(StartPosition.x, StartPosition.y);
        this.transform.position = worldPosition;

        for (int i = 0; i < _ghosts.Count; i++)
        {
            Destroy(_ghosts[i]);
        }
        _ghosts.Clear();

        if (_ghostItem != null)
        {
            _items.Add(_ghostItem);
            //ToDo: Tell Inventory something changed

            Destroy(_ghostItem);
            _ghostItem = null;
        }

        ActionPoints = Statics.ActionPoints;

        LastAction = false;
        RoundFinished = false;
        Active = false;

        Debug.Log(name + " has " + _items.Count + "items");
    }

    public void MessageHandler(string msg)
    {
        if (msg.Equals("NewRound"))
        {
            Debug.Log("Restart received" + _ghosts.Count);
            RoundStart();
        }
    }

    public void RemoveItem(GameObject item)
    {
        _items.Remove(item);
    }

}

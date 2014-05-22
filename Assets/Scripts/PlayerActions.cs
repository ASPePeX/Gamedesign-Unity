using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject PlayerGhost;

    private bool _active;
    private bool _roundFinished;

    private int _actionPoints;

    private IntVector2 _startPosition;
    private IntVector2 _finalPosition;
    private List<GameObject> _ghosts;

    private GameObject _level;


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

    public void AddGhost(IntVector2 instantiatePosition)
    {
        Vector2 posXY = Statics.TileToPos(instantiatePosition.x, instantiatePosition.y);

        var newGhost = Instantiate(PlayerGhost, new Vector3(posXY.x, posXY.y, 0), Quaternion.identity) as GameObject;
        newGhost.transform.parent = this.transform;
        _ghosts.Add(newGhost);
    }

    public bool CheckIfGhost(IntVector2 checkPosition)
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

    public IntVector2 RemoveGhosts(IntVector2 removePosition)
    {
        bool found = false;
        for (int i = _ghosts.Count - 1; i > -1; i--)
        {
            if (Statics.PosToTile(_ghosts[i].transform.position.x, _ghosts[i].transform.position.y).Equals(removePosition))
            {
                found = true;
            }

            Destroy(_ghosts[i]);
            _ghosts.RemoveAt(i);

            if (found)
            {
                if (i > 0)
                {
                    FinalPosition = Statics.PosToTile(_ghosts[i - 1].transform.position.x, _ghosts[i - 1].transform.position.y);
                    ActionPoints = Statics.ActionPoints - Statics.MovingToTileCost(Statics.PosToTile(_ghosts[i - 1].transform.position.x, _ghosts[i - 1].transform.position.y), Statics.PosToTile(this.transform.position.x, this.transform.position.y));
                }
                else
                {
                    RoundStart();
                }


                break;
            }

        }
        return FinalPosition;
    }

    public void ClearGhosts()
    {
        RoundStart();
    }

    // Use this for initialization
	void Start ()
	{
	    _level = GameObject.Find("/Level");
        _ghosts = new List<GameObject>();

        RoundStart();
	}
	
	// Update is called once per frame
/*	void Update () {

	    if (_active)
	    {
	        if (Input.GetMouseButton(0))
	        {
	            RoundStart();
                showMovement();
	        }
	    }
	}*/

    private void showMovement()
    {
       SendMessageUpwards("DrawMovement", FinalPosition.x + "x" + FinalPosition.y + "x" + ActionPoints, SendMessageOptions.RequireReceiver);
    }

    //called by the rounds manager (todo)
    public void RoundStart()
    {
        StartPosition = Statics.PosToTile(this.transform.position.x, this.transform.position.y);
        FinalPosition = StartPosition;

        for (int i = 0; i < _ghosts.Count; i++)
        {
            Destroy(_ghosts[i]);
        }
        _ghosts.Clear();

        ActionPoints = Statics.ActionPoints;

        RoundFinished = false;
    }
}

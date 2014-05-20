using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private bool _active;
    private bool _roundFinished;

    private int _actionPoints;

    private IntVector2 _startPosition;
    private IntVector2 _finalPosition;

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

    // Use this for initialization
	void Start ()
	{
	    _level = GameObject.Find("/Level");

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

        ActionPoints = Statics.ActionPoints;

        RoundFinished = false;
    }
}

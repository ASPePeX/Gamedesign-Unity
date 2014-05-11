using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private bool _active;
    private bool _roundFinished;

    private int _actionPoints;

    private IntVector2 _startPosition;
    private IntVector2 _finalPosition;

    private GameObject _level;

    // Use this for initialization
	void Start ()
	{
	    _active = true;
	    _level = GameObject.Find("/Level");

        RoundStart();
	}
	
	// Update is called once per frame
	void Update () {

	    if (_active)
	    {
	        if (Input.GetMouseButton(0))
	        {
	            RoundStart();
                showMovement();
	        }
	    }
	}

    private void showMovement()
    {
       SendMessageUpwards("DrawMovement", _finalPosition.x + "x" + _finalPosition.y + "x" + _actionPoints, SendMessageOptions.RequireReceiver);
    }

    //called by the rounds manager (todo)
    public void RoundStart()
    {
        _startPosition = Statics.PosToTile(this.transform.position.x, this.transform.position.y);
        _finalPosition = _startPosition;

        _actionPoints = Statics.ActionPoints;

        _roundFinished = false;
    }
}

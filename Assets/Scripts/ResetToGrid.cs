using UnityEngine;
using System.Collections;

public class ResetToGrid : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    transform.position = Statics.TileToPos(Statics.PosToTile(transform.position.x, transform.position.y));
	}
}

using System;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Collections;

public class ActionQueue : MonoBehaviour
{
    private BehaviorLevel _level;
    private List<ActionEntry> _actions;

	// Use this for initialization
	void Start ()
	{
	    _level = this.GetComponent<BehaviorLevel>();
        _actions = new List<ActionEntry>();
	}

    public void EvaluateActions()
    {
        foreach (ActionEntry ae in _actions)
        {
            if (ValidateAction(ae))
            {
                ExecuteAction(ae);
            }
        }
        _actions.Clear();
    }

    public void AddAction(GameObject goFrom, GameObject item, GameObject goTo)
    {
        _actions.Add(new ActionEntry(goFrom, item, goTo));
    }

    public void RemoveAction(GameObject player)
    {
        foreach (ActionEntry ae in _actions)
        {
            if (ae.GoFrom.Equals(player))
            {
                _actions.Remove(ae);
            }
        }    
    }

    private bool ValidateAction(ActionEntry ae)
    {
        if (ae.Item.GetComponent<ItemProperties>().RangeInActionpoints <= Statics.MovingToTileCost(ae.GoFrom, ae.GoTo))
        {
            return true;
        }
        return false;
    }

    private void ExecuteAction(ActionEntry ae)
    {
        
    }
}

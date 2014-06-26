using System;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

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
            ExecuteAction(ae);
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


    private void ExecuteAction(ActionEntry ae)
    {

        ItemProperties ItemScript = ae.Item.GetComponent<ItemProperties>();

        if (ItemScript.RangeInActionpoints <= Statics.MovingToTileCost(ae.GoFrom, ae.GoTo))
        {
            if (ItemScript.canHeal)
            {
                if (ae.GoFrom.CompareTag("Player") && ae.GoTo.CompareTag("Player"))
                {
                    ae.GoTo.GetComponent<PlayerActions>().HealthPoints += ItemScript.healAmount;
                    ae.GoFrom.GetComponent<PlayerActions>().RemoveItem(ae.Item);
                    Destroy(ae.Item);
                }
            }
            else if (ItemScript.doesDesinfect)
            {
                if (ae.GoFrom.CompareTag("Player") && ae.GoTo.CompareTag("Player"))
                {
                    ae.GoFrom.GetComponent<PlayerActions>().IsInfected = false;
                    ae.GoFrom.GetComponent<PlayerActions>().RemoveItem(ae.Item);
                    Destroy(ae.Item);
                }
            }
            else if (ItemScript.isWeapon)
            {
                if (ae.GoFrom.CompareTag("Player") && ae.GoTo.CompareTag("Enemy"))
                {
                    ae.GoTo.GetComponent<EnemyAction>().HealthPoints -= ItemScript.damageAmount;

                    if (ItemScript.isSecondaryWeapon)
                    {
                        ItemScript.ammoAmount -= 1;
                    }
                }
                else if (ae.GoFrom.CompareTag("Enemy") && ae.GoTo.CompareTag("Player"))
                {
                    var goToScript = ae.GoTo.GetComponent<PlayerActions>();

                    goToScript.HealthPoints -= (int) (ItemScript.damageAmount * (1 - goToScript.Items[goToScript.ActiveArmor].GetComponent<ItemProperties>().damageResistance));

                    if (ItemScript.canInfect)
                    {
                        if (Random.value < ItemScript.infectionChance)
                        {
                            goToScript.IsInfected = true;
                        }
                    }
                }
            }
        }
    }
}

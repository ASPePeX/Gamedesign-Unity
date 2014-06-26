using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    private List<ActionEntry> _actions;

    // Use this for initialization
    private void Start()
    {
        _actions = new List<ActionEntry>();
    }

    public void EvaluateActions()
    {
        Debug.Log("Evaluating Actions!");
        foreach (ActionEntry ae in _actions)
        {
            ExecuteAction(ae);
        }
        _actions.Clear();
    }

    public void AddAction(GameObject goFrom, GameObject item, GameObject goTo)
    {
        _actions.Add(new ActionEntry(goFrom, item, goTo));
        Debug.Log("Added Action to Queue!");
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
        Debug.Log("Executing Action!");
        var ItemScript = ae.Item.GetComponent<ItemProperties>();

        if (ItemScript.RangeInActionpoints >= Statics.MovingToTileCost(ae.GoFrom, ae.GoTo))
        {
            Debug.Log("Enought Action Points! gogogo");
            if (ItemScript.canHeal)
            {
                if (ae.GoFrom.CompareTag("Player") && ae.GoTo.CompareTag("Player"))
                {
                    Debug.Log("Healing!");
                    ae.GoTo.GetComponent<PlayerActions>().HealthPoints += ItemScript.healAmount;
                    ae.GoFrom.GetComponent<PlayerActions>().RemoveItem(ae.Item);
                    Destroy(ae.Item);
                }
            }
            if (ItemScript.doesDesinfect)
            {
                if (ae.GoFrom.CompareTag("Player") && ae.GoTo.CompareTag("Player"))
                {
                    ae.GoFrom.GetComponent<PlayerActions>().IsInfected = false;
                    ae.GoFrom.GetComponent<PlayerActions>().RemoveItem(ae.Item);
                    Destroy(ae.Item);
                }
            }
            if (ItemScript.isWeapon)
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

                    goToScript.HealthPoints -= (int) (ItemScript.damageAmount*(1 - goToScript.Items[goToScript.ActiveArmor].GetComponent<ItemProperties>().damageResistance));

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
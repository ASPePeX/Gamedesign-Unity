using System;
using UnityEngine;
using System.Collections;

public class ItemProperties : MonoBehaviour
{
    public int RangeInActionpoints;
    
    public bool canHeal;
    public int healAmount;

    public bool doesDesinfect;

    public bool isWeapon;
    public int damageAmount;

    public bool isSecondaryWeapon;
    public int ammoAmount;

    public bool isArmor;
    public float damageResistance;
    public float infectionResistance;

    //Enemy Damage
    public bool canInfect;
    public float infectionChance;
}
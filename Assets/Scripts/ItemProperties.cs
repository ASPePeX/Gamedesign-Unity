using UnityEngine;
using System.Collections;

public class ItemProperties : MonoBehaviour
{
    public bool canHeal;
    public int healAmount;

    public bool doesDesinfect;

    public bool isWeapon;
    public int damageAmount;
    public int weaponRange;

    public bool isSecondaryWeapon;
    public int ammoAmount;

    public bool canInfect;
    public float infectionChance;

    public bool isArmor;
    public float damageReduction;
}
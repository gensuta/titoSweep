using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "SOs/Weapon", order = 3)]
public class Weapon : ScriptableObject
{
    public WeaponType type;

    public WeaponType typeWeakness; // not valid for all

    //hit% 

    //

    // TODO: Add more to this plz. name, icon, what have you
}

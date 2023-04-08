using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "SOs/Class", order = 0)]
public class CharacterClass : ScriptableObject
{
    public string Name;

    public bool canFly;
    public int[] baseStats; // 0 HP,1 STR,2 MGC,3 SKL,4 SPD,5 LCK,6 DEF,7 RES,8 MOV,9 CON

    public WeaponType weaponWeakness; // not valid for all

    public List<WeaponProficiency> proficientWeapons; // weapon rank is a scriptable object that has a list of weapons and an enum for actual rank (D, C, B, A, S )

}

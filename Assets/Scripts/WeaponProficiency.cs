using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProficiencyLevel { D, C, B, A, S }
public enum WeaponType { None,Sword,Lance,Bow, Staff, Magic, Dagger }

[CreateAssetMenu(fileName = "WeaponProficiency", menuName = "SOs/Weapon Proficiency", order = 2)]
// to be given to characters. Not actual weapons. Weapons will also be SOs
public class WeaponProficiency : ScriptableObject
{
    public ProficiencyLevel level;
    public WeaponType type;

}

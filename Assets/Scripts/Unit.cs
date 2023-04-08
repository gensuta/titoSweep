using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction
{
    Hero,
    Ally,
    Recruitable, //same as ally but can become a hero
    Enemy
}
[CreateAssetMenu(fileName = "Unit", menuName = "SOs/Unit",order = 1)]
public class Unit : ScriptableObject // contains stats, sprites, and string info ( name/desc)
{
    [Header("Unit Info")]

    public string Name;
    public string Description;

    [Header("STATS")]

    public int MAX_HP;
    public int HP;

    public int STR, MGC, SKL, SPD, LCK, DEF, RES, MOV, CON; // Unit stats

    [Header("Art Assets")]

    public Sprite unitIcon;

    [Header("Other classifying info")]

    public Faction faction;
    public CharacterClass characterClass;

    public List<WeaponProficiency> extraWeaponProficiences; // for characters that go above the ranks of their class etc

    public int level = 1;

    public void LevelUp(int[] statGrowth)
    {
        int[] currentStats = new int[] { STR, MGC,SKL,SPD,LCK,DEF,RES,MOV,CON };

        for (int i = 0; i < statGrowth.Length; i++) 
        {
            currentStats[i] += statGrowth[i];
        }
        level++;
        UpdateStats(currentStats);
    }

    public void SetBaseStats()
    {
        UpdateStats(characterClass.baseStats);
        this.HP = this.MAX_HP;
    }

    private void UpdateStats(int[] newStats)
    {
        MAX_HP = newStats[0];
        STR = newStats[1];
        MGC = newStats[2];
        SKL = newStats[3];
        SPD = newStats[4];
        LCK = newStats[5];
        DEF = newStats[6];
        RES = newStats[7];
        MOV = newStats[8];
        CON = newStats[9];
    }
}

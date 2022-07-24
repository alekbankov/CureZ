using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleData
{
    public static float ItemDamage = 0;
    public static bool NightTime = false;
    public static int ZombieLevel = 0;
    public static float ZombieDamage = 0;

    public static void ResetData()
    {
        ItemDamage = ZombieLevel = 0;
        NightTime = false;
    }
}

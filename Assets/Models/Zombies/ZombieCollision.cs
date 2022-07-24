using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieCollision : MonoBehaviour
{

    public int level, damage;
    private void OnTriggerEnter(Collider collision)
     {
         Debug.Log("collision");
         if (collision.gameObject.name == "Player")
         {
            // BattleData.ZombieLevel = level;
           //  BattleData.ZombieDamage = damage;
             SceneManager.LoadScene("BattleLobby");
             Destroy(gameObject);
         }
     }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = System.Random;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST, BONUS }
public class BattleManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattlePosition;
    public Transform enemyBattlePosition;
    public Transform screenPosition;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    private CharacterStatus playerStatus;
    private CharacterStatus enemyStatus;
    private bool revive, damageBonus;
    private float bonusHeal;
    
    
    private GameObject _canvas;

    public BattleState state;
    
    public Text dialogueText;

    public GameObject Victory_Screen, Loss_Screen;

    
#region SetUp    
    void Start()
    {
        state = BattleState.START;
        revive = true;
        damageBonus = false;
        _canvas = GameObject.Find("Canvas");
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattlePosition);
        playerGO.transform.SetParent(_canvas.transform);
        playerStatus = playerGO.GetComponent<CharacterStatus>();
        playerHUD.SetHUD(playerStatus);

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattlePosition);
        enemyGO.transform.SetParent(_canvas.transform);
        enemyStatus = enemyGO.GetComponent<CharacterStatus>();
        enemyHUD.SetHUD(enemyStatus);

        dialogueText.text = "Prepare for battle!";
        yield return new WaitForSeconds(1.5f);

        dialogueText.text = "Your turn";
        state = BattleState.PLAYERTURN;
    }
#endregion

#region Battle
    IEnumerator PlayerAttack()
    {
        int bonus = 0;
        if (damageBonus)
        {
            bonus = (int)playerStatus.damage / 10;
        }
        bool isDead = enemyStatus.TakeDamage(playerStatus.damage + bonus);
        
        enemyHUD.SetHP(enemyStatus.currentHealth, enemyStatus.maxHealth);
        if (damageBonus)
        {
            dialogueText.text = "What a blow!";
            damageBonus = false;
        }
        else
        {
            dialogueText.text = "Attack is successful!";
        }
        
        yield return new WaitForSeconds(1.5f);

        if (isDead)
        {
            state = BattleState.WIN;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerStatus.Heal(10);
        
        playerHUD.SetHP(playerStatus.currentHealth, playerStatus.maxHealth);
        dialogueText.text = "HP healed!";

        yield return new WaitForSeconds(1.5f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = "Enemy attacks";

        yield return new WaitForSeconds(1.5f);

        bool isDead = playerStatus.TakeDamage(enemyStatus.damage);
        playerHUD.SetHP(playerStatus.currentHealth, playerStatus.maxHealth);
        
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.BONUS;
            StartCoroutine(TeamBonus());
        }
    }

    IEnumerator TeamBonus()
    {
        int index = UnityEngine.Random.Range(0, 6);
        if (index == 1)
        {
            dialogueText.text = "Team Bonus!";
            yield return new WaitForSeconds(0.5f);
            
            playerStatus.Heal(playerStatus.maxHealth/10);
            playerHUD.SetHP(playerStatus.currentHealth, playerStatus.maxHealth);
            dialogueText.text = "Allied health healed!";
        }
        if (index == 0)
        {
            dialogueText.text = "Team Bonus!";
            yield return new WaitForSeconds(0.5f);
            
            dialogueText.text = "Damage bonus!";
            damageBonus = true;
        }

        yield return new WaitForSeconds(1f);
        dialogueText.text = "Your turn";
        state = BattleState.PLAYERTURN;
    }


    string CalculateBonus()
    {
        int stone = UnityEngine.Random.Range(enemyStatus.level - 8, enemyStatus.level + 10);
        int wood = UnityEngine.Random.Range(enemyStatus.level - 8, enemyStatus.level + 10);
        int coin = UnityEngine.Random.Range(enemyStatus.level - 8, enemyStatus.level + 10);
        StartCoroutine(ResourceUpdate(wood, stone, coin));
        return "stone: " + stone + "\n wood: " + wood + "\n coin: " + coin;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }
#endregion

#region BattleEnd
    void EndBattle()
    {
        if (state == BattleState.WIN)
        {
            dialogueText.text = "You are victorious!";
            GameObject screen = Instantiate(Victory_Screen, screenPosition);
            screen.transform.SetParent(_canvas.transform);
            Text txt = screen.GetComponentInChildren<Text>();
            txt.text = CalculateBonus();
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Battle lost";
            GameObject screen = Instantiate(Loss_Screen, screenPosition);
            screen.transform.SetParent(_canvas.transform);
        }
    }

    IEnumerator ResourceUpdate(int wood, int stone, int coin)
    {
        string uri = "";
        WWWForm form = new WWWForm();
        form.AddField("Wood", wood);
        form.AddField("stone", stone);
        form.AddField("coin", coin);
        
        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();
            
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError ||
                www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Resource updated successfully");
            }
        }
    }
#endregion    
}

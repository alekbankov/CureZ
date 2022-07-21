using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST }
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
    
    
    private GameObject _canvas;

    public BattleState state;
    
    public Text dialogueText;

    public GameObject Victory_Screen, Loss_Screen;

    
    

    private void Start()
    {
        state = BattleState.START;
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

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyStatus.TakeDamage(playerStatus.damage);
        
        enemyHUD.SetHP(enemyStatus.currentHealth, enemyStatus.maxHealth);
        dialogueText.text = "Attack is successful!";
        
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
            dialogueText.text = "Your turn";
            state = BattleState.PLAYERTURN;
        }
    }

    IEnumerator Pause(string message)
    {
        dialogueText.text = message;
        yield return new WaitForSeconds(1.5f);
    }

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

    string CalculateBonus()
    {
        int stone = UnityEngine.Random.Range(enemyStatus.level - 8, enemyStatus.level + 10);
        int wood = UnityEngine.Random.Range(enemyStatus.level - 8, enemyStatus.level + 10);
        int coin = UnityEngine.Random.Range(enemyStatus.level - 8, enemyStatus.level + 10);
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
}

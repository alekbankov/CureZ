using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBonus : MonoBehaviour
{
    [SerializeField] private int bonus = 10;

    private void OnMouseDown() {
        GameManager.Instance.CurrentPlayer.addXp(bonus);
        Destroy(gameObject);
    }
}

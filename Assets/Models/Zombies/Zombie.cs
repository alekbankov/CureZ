using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    public Transform player;
    Rigidbody rb;

    [SerializeField] private float spawnRate = 0.5f;
    [SerializeField] private int attack = 0;

    [SerializeField] private int defense = 0;

    [SerializeField] private int hp = 10;

    [SerializeField] private int speed = 5;

    public float SpawnRate {
        get {return spawnRate;}
    }

    public int Attack {
        get {return attack;}
    }

    public int Defense {
        get {return defense;}
    }

    public int Hp {
        get {return hp;}
    }

    private void FollowPlayer() {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Update is called once per frame
    void FixedUpdate()
    {

        //Make the enemy move
            Vector3 pos = Vector3.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
            rb.MovePosition(pos);
            transform.LookAt(player);
    }
}

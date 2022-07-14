using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [SerializeField] private int xp = 0;
    [SerializeField] private int requiredXp = 100;
    [SerializeField] private int levelBase = 100;
    [SerializeField] private int health = 100;
    [SerializeField] private List<GameObject> cars = new List<GameObject>();

    private int level = 1;

    public int Xp() {
        return xp;
    }

    public int RequiredXp {
        get {return requiredXp;}
    }

    public int Health{
        get {return health;}
    }

    public int LevelBase{
        get {return levelBase;}
    }

    public List<GameObject> Cars{
        get{return cars;}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Move;

public class Revenge : MonoBehaviour, Move
{
    public List<moveStat> moveStats { get; set; } = new List<moveStat>();
    public int level;
    [field: SerializeField]
    public string moveName { get; set; }
    [field: SerializeField]
    public string moveType { get; set; }

    public int totalDamage { get; set; }
    public float timeActive { get; set; }
    [field: SerializeField]
    public int baseDmg { get; set; }
    [field: SerializeField]
    public float coolDown { get; set; }
    public PartyController partyController { get; set; }

    [field: SerializeField]
    public int baseDmgPoints { get { return moveStats[0].statPoints; } set { } }
    [field: SerializeField]
    public int dRPoints { get { return moveStats[1].statPoints; } set { } }
    public int sizePoints { get { return moveStats[2].statPoints; } set { } }

    public PokeDex.PokeDexEntry pokemon { get; set; }

    public float damage { get { return moveStats[0].Calc(baseDmgPoints); } private set { } }
    public float nextLeveldamage { get { return moveStats[0].Calc(baseDmgPoints + 1); } private set { } }
    public float damageReduction { get { return moveStats[1].Calc(dRPoints); } private set { } }
    public float nextLevelDamageReduction { get { return moveStats[1].Calc(dRPoints + 1); } private set { } }
    public float size { get { return moveStats[2].Calc(sizePoints); } private set { } }
    public float nextLevelsize { get { return moveStats[2].Calc(sizePoints + 1); } private set { } }

    float nextAttack = 0;
    float nextTick = 0;
    public bool activate;

    List<mob> hitMobs = new List<mob>();
    public PokeDex pokeDex { get; set; }
    public Player player { get; set; }

    public Collider2D hitBox;
    public GameObject icon;
    float disableTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Damage");
        moveStats[0].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(player.thunderStoneAmount))) * 4 *(pokemon.@base.Attack + player.might + 100f) / (50 + 100) * (x + 1)); };
        addMoveStat("Damage Reduction");
        moveStats[1].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(0))) * (pokemon.@base.HP + 75f) / (50 + 100) * (x + 1)); };
        addMoveStat("Size");
        moveStats[2].Calc = (x) => { return (float)(math.pow(1.05, CalcMulti(0))) * (.8f * (pokemon.@base.Defense+350) / (40f + 350)) * ((float)(math.log10(x + 1)) + 1 + .05f * x); };
        partyController = GameObject.Find("Party").GetComponent<PartyController>();
        nextTick = Time.time;
        icon.SetActive(false);
        disableTime = Time.time;
    }

    public int MultiAmount;
    int CalcMulti(int stoneAmount)
    {
        int multi = 0;
        multi += stoneAmount;
        if (pokemon.evolution.next != null)
        {
            multi += player.everStoneAmount;
        }
        if ((pokemon.@base.HP + pokemon.@base.Attack + pokemon.@base.Defense + pokemon.@base.SpAttack + pokemon.@base.SpDefense + pokemon.@base.Speed) < 500)
        {
            multi += player.commonBoostAmount;
        }
        else
        if ((pokemon.@base.HP + pokemon.@base.Attack + pokemon.@base.Defense + pokemon.@base.SpAttack + pokemon.@base.SpDefense + pokemon.@base.Speed) < 550)
        {
            multi += player.rareBoostAmount;
        }
        MultiAmount = multi;
        return multi;
    }
    public void addMoveStat(string name)
    {
        moveStat m = new moveStat();
        m.statName = name;
        moveStats.Add(m);
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            icon.SetActive(true);
            disableTime = Time.time + .5f;
            foreach (mob m in hitMobs) 
            {
                if (damage < m.currentHp)
                {
                    totalDamage += (int)damage;
                }
                else
                {
                    totalDamage += (int)m.currentHp;
                }
                m.Damage((int)damage);
            }
            activate = false;
        }
        if (Time.time > disableTime)
        {
            icon.SetActive(false);
        }
        gameObject.transform.localScale = new Vector3(size, size, 1);
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mob m = collision.GetComponent<mob>();
        if(m != null)
        {
            hitMobs.Add(collision.gameObject.GetComponent<mob>());
        }
        //Debug.Log("add");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        mob m = collision.GetComponent<mob>();
        if (m != null)
        {
            hitMobs.Remove(collision.gameObject.GetComponent<mob>());
        }
    }
}

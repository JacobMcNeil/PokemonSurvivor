using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Move;

public class Toxic : MonoBehaviour,Move
{
    public int level;
    [field: SerializeField]
    public string moveName { get; set; }
    [field: SerializeField]
    public string moveType { get; set; }
    [field: SerializeField]
    public PokeDex.PokeDexEntry pokemon { get; set; }

    public int totalDamage { get; set; }
    public float timeActive { get; set; }
    public PartyController partyController { get; set; }


    public float damage { get { return moveStats[0].Calc(baseDmgPoints); } private set { } }
    public float nextLeveldamage { get { return moveStats[0].Calc(baseDmgPoints + 1); } private set { } }
    public float cooldown { get { return moveStats[1].Calc(coolDownPoints); } private set { } }
    public float nextLevelcooldown { get { return moveStats[1].Calc(coolDownPoints + 1); } private set { } }
    public float size { get { return moveStats[2].Calc(sizePoints); } private set { } }
    public float nextLevelsize { get { return moveStats[2].Calc(sizePoints + 1); } private set { } }

    float duration = 5f;
    public List<moveStat> moveStats { get; set; } = new List<moveStat>();
    public int baseDmgPoints { get { return moveStats[0].statPoints; } set { } }
    public int coolDownPoints { get { return moveStats[1].statPoints; } set { } }
    public int sizePoints { get { return moveStats[2].statPoints; } set { } }


    float nextAttack = 0;
    public float tickRate = .25f;

    List<mob> hitMobs = new List<mob>();

    public GameObject toxicInstace;
    public Player player { get; set; }

    public PokeDex pokeDex { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Damage");
        moveStats[0].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(player.toxicBarbAmount))) * (pokemon.@base.Attack + player.might + 100f) / (50) * (x + 1)); };
        addMoveStat("Cooldown");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (1.5f * 100f / pokemon.@base.Speed) * ((float)(math.pow(.9, x))); };
        addMoveStat("Size");
        moveStats[2].Calc = (x) => { return (float)(math.pow(1.05, CalcMulti(0))) * ((pokemon.@base.Defense + 350) / (40f + 350)) * ((float)(math.log10(x + 1)) + 1 + .05f * x); };
        partyController = GameObject.Find("Party").GetComponent<PartyController>();
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
        if (Time.time > nextAttack)
        {
            Attack();
        }
    }

    public void Attack()
    {
        //Debug.Log("Attack");
        GameObject t = Instantiate(toxicInstace);
        t.GetComponent<ToxicInstance>().toxic = this;
        t.GetComponent<ToxicInstance>().goAwayTime = Time.time + duration;
        t.transform.SetAsFirstSibling();


        Vector3 spawnlocation = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f));
        t.transform.position = spawnlocation.normalized * 3.5f + player.transform.position + new Vector3(0,0,.005f);
        t.transform.localScale = new Vector3(size, size, 1);
        nextAttack = Time.time + (cooldown);

    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Move;

public class RazorLeaf : MonoBehaviour, Move
{
    public List<moveStat> moveStats { get; set; } = new List<moveStat>();
    public int level;
    [field: SerializeField]
    public string moveName { get; set; }

    public int totalDamage { get; set; }
    public float timeActive { get; set; }
    [field: SerializeField]
    public string moveType { get; set; }
    public PokeDex.PokeDexEntry pokemon { get; set; }

    float nextAttack = 0;
    float nextTick = 0;

    List<mob> hitMobs = new List<mob>();
    public PartyController partyController { get; set; }

    public GameObject razorLeaf;
    public Player player { get; set; }



    public float damage { get { return moveStats[0].Calc(baseDmgPoints); } private set { } }
    public float nextLeveldamage { get { return moveStats[0].Calc(baseDmgPoints + 1); } private set { } }
    public float cooldown { get { return moveStats[1].Calc(coolDownPoints); } private set { } }
    public float nextLevelcooldown { get { return moveStats[1].Calc(coolDownPoints + 1); } private set { } }
    public float pierce { get { return moveStats[2].Calc(piercePoints); } private set { } }
    public float nextLevelPierce { get { return moveStats[2].Calc(piercePoints + 1); } private set { } }

    public int baseDmgPoints { get { return moveStats[0].statPoints; } set { } }
    public int coolDownPoints { get { return moveStats[1].statPoints; } set { } }
    public int piercePoints { get { return moveStats[2].statPoints; } set { } }
    public PokeDex pokeDex { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Damage");
        moveStats[0].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(player.leafStoneAmount))) * (pokemon.@base.Attack + player.might + 200f) * 2 / (50) * (x + 1)); };
        addMoveStat("Cooldown");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (.3f * 100f / pokemon.@base.Speed) * ((float)(Unity.Mathematics.math.pow(.9, x))); };
        addMoveStat("Pierce");
        moveStats[2].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(0))) * (pokemon.@base.Defense + 75f) / (50 + 100) * (x + 1)); };
        partyController = GameObject.Find("Party").GetComponent<PartyController>();
    }
    public int MultiAmount;
    int CalcMulti(int stoneAmount)
    {
        int multi = 0;
        multi += stoneAmount;
        if (pokemon.evolution.next != null)
        {
            Debug.Log("everstone: " + player.everStoneAmount);
            multi += player.everStoneAmount;
        }
        if ((pokemon.@base.HP + pokemon.@base.Attack + pokemon.@base.Defense + pokemon.@base.SpAttack + pokemon.@base.SpDefense + pokemon.@base.Speed) < 500)
        {
            Debug.Log("commonBoostAmount: " + player.commonBoostAmount);
            multi += player.commonBoostAmount;
        }
        else
        if ((pokemon.@base.HP + pokemon.@base.Attack + pokemon.@base.Defense + pokemon.@base.SpAttack + pokemon.@base.SpDefense + pokemon.@base.Speed) < 550)
        {
            Debug.Log("rareBoostAmount: " + player.rareBoostAmount);
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
        GameObject b = Instantiate(razorLeaf);
        RazorLeafInstance ri = b.GetComponent<RazorLeafInstance>();
        ri.bulbasaur = this;
        ri.pierce = pierce;
        ri.goAwayTime = Time.time + 4; ;

        float flip = 1;
        if (player.forward.y < 0)
        {
            flip = -1;
        }
        b.transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(player.forward, Vector2.right) * flip);
        b.transform.position = gameObject.transform.position;
        b.transform.position += new Vector3(1,1,0) * (UnityEngine.Random.value - .5f);

        nextAttack = Time.time + (cooldown);

    }

}

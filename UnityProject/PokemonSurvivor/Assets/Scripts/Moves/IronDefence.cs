using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Move;

public class IronDefence : MonoBehaviour, Move
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
    public PartyController partyController { get; set; }

    [field: SerializeField]
    public int maxStacksPoints { get { return moveStats[0].statPoints; } set { } }
    [field: SerializeField]
    public int cooldownPoints { get { return moveStats[1].statPoints; } set { } }
    public int sizePoints { get { return moveStats[2].statPoints; } set { } }

    public PokeDex.PokeDexEntry pokemon { get; set; }

    public int currentStacks;
    public float maxStacks { get { return moveStats[0].Calc(maxStacksPoints); } private set { } }
    public float nextLevelmaxStacks { get { return moveStats[0].Calc(maxStacksPoints + 1); } private set { } }
    public float cooldown { get { return moveStats[1].Calc(cooldownPoints); } private set { } }
    public float nextLevelcooldown { get { return moveStats[1].Calc(cooldownPoints + 1); } private set { } }
    public float size { get { return moveStats[2].Calc(sizePoints); } private set { } }
    public float nextLevelsize { get { return moveStats[2].Calc(sizePoints + 1); } private set { } }

    float nextAttack = 0;
    float nextTick = 0;
    public bool activate;

    List<mob> hitMobs = new List<mob>();
    public PokeDex pokeDex { get; set; }
    public Player player { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Max Stacks");
        moveStats[0].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(player.thunderStoneAmount))) * 4 *(pokemon.@base.HP + player.might + 100f) / (50 + 100) * (x + 1)); };
        addMoveStat("Cooldown");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (1.5f * 100f / pokemon.@base.Defense) * ((float)(math.pow(.9, x))); };
        addMoveStat("Size");
        moveStats[2].Calc = (x) => { return (float)(math.pow(1.05, CalcMulti(0))) * (.8f * (pokemon.@base.Defense+350) / (40f + 350)) * ((float)(math.log10(x + 1)) + 1 + .05f * x); };
        partyController = GameObject.Find("Party").GetComponent<PartyController>();
        nextTick = Time.time;
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
        if(currentStacks < maxStacks)
        {
            currentStacks += 1;
        }

        nextAttack = Time.time + (cooldown);

    }
}

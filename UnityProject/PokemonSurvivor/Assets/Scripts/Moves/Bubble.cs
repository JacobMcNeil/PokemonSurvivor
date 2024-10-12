using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Move;

public class Bubble : MonoBehaviour, Move
{
    public int level;
    [field: SerializeField]
    public string moveName { get; set; }
    [field: SerializeField]
    public PokeDex.PokeDexEntry pokemon { get; set; }

    [field: SerializeField]
    public string moveType { get; set; }

    public int totalDamage { get; set; }
    public float timeActive { get; set; }

    public float damage { get { return moveStats[0].Calc(baseDmgPoints); } private set { } }
    public float nextLeveldamage { get { return moveStats[0].Calc(baseDmgPoints + 1); } private set { } }
    public float cooldown { get { return moveStats[1].Calc(coolDownPoints); } private set { } }
    public float nextLevelcooldown { get { return moveStats[1].Calc(coolDownPoints + 1); } private set { } }
    public float speed { get { return moveStats[2].Calc(speedPoints); } private set { } }
    public float nextLevelspeed { get { return moveStats[2].Calc(speedPoints + 1); } private set { } }

    public List<moveStat> moveStats { get; set; } = new List<moveStat>();
    public int baseDmgPoints { get { return moveStats[0].statPoints; } set { } }
    public int coolDownPoints { get { return moveStats[1].statPoints; } set { } }
    public int speedPoints { get { return moveStats[2].statPoints; } set { } }


    float nextAttack = 0;
    float nextTick = 0;

    List<mob> hitMobs = new List<mob>();

    public GameObject bubble;
    public Player player { get; set; }

    public PokeDex pokeDex { get; set; }
    public PartyController partyController { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Damage");
        moveStats[0].Calc = (x) => 
        {
            return  Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(player.waterStoneAmount))) * (pokemon.@base.Attack + player.might + 200f) / (50) * (x + 1));
        };
        addMoveStat("Cooldown");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (.75f * 100f / pokemon.@base.Speed) * ((float)(math.pow(.9, x))); };
        addMoveStat("Speed");
        moveStats[2].Calc = (x) => { return (float)(math.pow(1.05, CalcMulti(0))) * (25f * pokemon.@base.SpAttack / 60) * ((float)(math.pow(1.1, x))); };
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
        GameObject b = Instantiate(bubble);
        b.transform.SetParent(gameObject.transform, false);
        b.GetComponent<BubbleInstance>().bubble = this;

        b = Instantiate(bubble);
        b.GetComponent<BubbleInstance>().bubble = this;
        b.transform.eulerAngles = new Vector3(0, 0, 180);
        b.transform.SetParent(gameObject.transform, false);
        nextAttack = Time.time + (cooldown);

    }

}

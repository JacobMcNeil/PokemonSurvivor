using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Move;

public class Rock : MonoBehaviour, Move
{
    public List<moveStat> moveStats { get; set; } = new List<moveStat>();
    public int level;
    [field: SerializeField]
    public string moveName { get; set; }

    public int totalDamage { get; set; }
    public float timeActive { get; set; }
    [field: SerializeField]
    public string moveType { get; set; }
    public int damagePoints { get { return moveStats[0].statPoints; } set { } }
    public int coolDownPoints { get { return moveStats[1].statPoints; } set { } }
    public int amountPoints { get { return moveStats[2].statPoints; } set { } }

    public float tickRate;
    public PokeDex.PokeDexEntry pokemon { get; set; }
    public PartyController partyController { get; set; }


    public float damage { get { return moveStats[0].Calc(damagePoints); } private set { } }
    public float nextLeveldamage { get { return moveStats[0].Calc(damagePoints + 1); } private set { } }
    public float cooldown { get { return moveStats[1].Calc(coolDownPoints); } private set { } }
    public float nextLevelcooldown { get { return moveStats[1].Calc(coolDownPoints + 1); } private set { } }
    public float amount { get { return moveStats[2].Calc(amountPoints); } private set { } }
    public float nextLevelamount { get { return moveStats[2].Calc(amountPoints + 1); } private set { } }

    float nextAttack;

    public GameObject rockInstance;
    public Player player { get; set; }
    public PokeDex pokeDex { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Damage");
        moveStats[0].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(player.hardStoneAmount))) * 7 *(4 * (pokemon.@base.Attack + player.might) + 700f) / (50 + 100) * (x + 1)); };
        addMoveStat("Cooldown");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (2 * 120f / pokemon.@base.Speed) * ((float)(math.pow(.9, x))); };
        addMoveStat("Amount");
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
        Debug.Log("Attack");
        for(int i = 0; i < amount; i++)
        {
            GameObject r = Instantiate(rockInstance);
            r.GetComponent<RockInstance>().rock = this;
            r.GetComponent<RockInstance>().goAwayTime = Time.time + cooldown;

            Vector3 spawnDirection = player.forward;
            Vector2 perpendicularDirection = Vector2.Perpendicular(spawnDirection);
            r.transform.position = (spawnDirection.normalized * 3 + player.transform.position + new Vector3(0, 0, 0)) + (Vector3)perpendicularDirection.normalized * Mathf.CeilToInt(i/2f) * ((i*1f)%2 -.5f) *2;
        }

        nextAttack = Time.time + (cooldown);

    }
}

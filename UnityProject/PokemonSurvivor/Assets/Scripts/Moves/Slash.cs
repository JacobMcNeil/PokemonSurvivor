using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Move;

public class Slash : MonoBehaviour, Move
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
    public PartyController partyController { get; set; }

    [field: SerializeField]
    public int baseDmgPoints { get { return moveStats[0].statPoints; } set { } }
    [field: SerializeField]
    public int coolDownPoints { get { return moveStats[1].statPoints; } set { } }
    public int sizePoints { get { return moveStats[2].statPoints; } set { } }

    public PokeDex.PokeDexEntry pokemon { get; set; }

    public float damage { get { return moveStats[0].Calc(baseDmgPoints); } private set { } }
    public float nextLeveldamage { get { return moveStats[0].Calc(baseDmgPoints + 1); } private set { } }
    public float coolDown { get { return moveStats[1].Calc(coolDownPoints); } private set { } }
    public float nextLevelCoolDown { get { return moveStats[1].Calc(coolDownPoints + 1); } private set { } }
    public float size { get { return moveStats[2].Calc(sizePoints); } private set { } }
    public float nextLevelsize { get { return moveStats[2].Calc(sizePoints + 1); } private set { } }

    float nextAttack = 0;
    float nextTick = 0;

    List<mob> hitMobs = new List<mob>();
    public PokeDex pokeDex { get; set; }
    public Player player { get; set; }

    public GameObject attackInstance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Damage");
        moveStats[0].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(0))) * 13f *(pokemon.@base.Attack + player.might + 100f) / (50 + 100) * (x + 1)); };
        addMoveStat("CoolDown");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (1f * 100f / pokemon.@base.Speed) * ((float)(math.pow(.9, x))); };
        addMoveStat("Size");
        moveStats[2].Calc = (x) => { return (float)(math.pow(1.05, CalcMulti(0))) * (.9f * (pokemon.@base.Defense+350) / (40f + 350)) * ((float)(math.log10(x + 1)) + 1 + .05f * x); };
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
        gameObject.transform.localScale = new Vector3(size, size, 1);

        if (Time.time > nextAttack)
        {
            Attack();
            nextAttack = Time.time + coolDown; 
        }
        float flip = 1;
        if (player.forward.y < 0)
        {
            flip = -1;
        }
        gameObject.transform.eulerAngles = new Vector3(0, 0, (Vector2.Angle(player.forward, Vector2.right) * flip) - 90f);
    }
    bool flipAttack = false;
    public void Attack()
    {

        GameObject a = GameObject.Instantiate(attackInstance);
        a.transform.SetParent(transform, false);
        a.GetComponent<SlashInstance>().slash = this;
        if (flipAttack)
        {
            a.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        flipAttack = !flipAttack;
    }


}

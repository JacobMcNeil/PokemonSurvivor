using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Move;

public class Ember : MonoBehaviour, Move
{
    public List<moveStat> moveStats { get; set; } = new List<moveStat>();
    public int level;
    [field: SerializeField]
    public string moveName { get; set; }

    public int totalDamage { get; set; }
    public float timeActive { get; set; }

    [field: SerializeField]
    public string moveType { get; set; }
    public int baseDmgPoints { get { return moveStats[0].statPoints; } set { } }
    public int coolDownPoints { get { return moveStats[1].statPoints; } set { } }
    public int amountPoints { get { return moveStats[2].statPoints; } set { } }

    public float tickRate;
    public PokeDex.PokeDexEntry pokemon { get; set; }
    public PartyController partyController { get; set; }


    public float damage { get { return moveStats[0].Calc(baseDmgPoints); } private set { } }
    public float nextLeveldamage { get { return moveStats[0].Calc(baseDmgPoints + 1); } private set { } }
    public float cooldown { get { return moveStats[1].Calc(coolDownPoints); } private set { } }
    public float nextLevelcooldown { get { return moveStats[1].Calc(coolDownPoints + 1); } private set { } }
    public float amount { get { return moveStats[2].Calc(amountPoints); } private set { } }
    public float nextLevelamount { get { return moveStats[2].Calc(amountPoints + 1); } private set { } }

    float nextAttack = 0;
    float nextTick = 0;

    List<mob> hitMobs = new List<mob>();

    //public GameObject flame;
    public GameObject emberInstance;
    public Player player { get; set; }
    float turnFlameOff;


    public PokeDex pokeDex { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Damage");
        moveStats[0].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(player.fireStoneAmount))) * 2 *(pokemon.@base.Attack + player.might + 100f) / (50 + 100) * (x + 1)); };
        addMoveStat("Cooldown");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (2 * 120f / pokemon.@base.Speed) * ((float)(math.pow(.9, x))); };
        addMoveStat("Amount");
        moveStats[2].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(0))) * (pokemon.@base.Defense + 75f) / (50 + 100) * (x + 1)); };
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
        if(Time.time > nextAttack)
        {
            Attack();
        }
        //if(Time.time > turnFlameOff)
        //{
        //    flame.SetActive(false);
        //    nextTick = Time.time;
        //}
        //if(Time.time > nextTick && Time.time < turnFlameOff)
        //{
        //    foreach (mob m in hitMobs)
        //    {
        //        //Debug.LogError(Mathf.FloorToInt((Time.time - nextTick) / tickRate));
        //        //Debug.LogError((Time.time - nextTick));
        //        //Debug.LogError(tickRate);
        //        //Debug.LogError(((Time.time - nextTick) / tickRate));
        //        for (int i = 0; i < 1 + Mathf.FloorToInt((Time.time - nextTick) / tickRate); i++)
        //        {
        //            m.Damage((int)damage);
        //            totalDamage += (int)damage;
        //            //Debug.Log(m.health);
        //        }
        //    }
        //    nextTick = Time.time + tickRate;
        //}
        //Debug.Log(Quaternion.Euler(player.forward));
        float flip = 1;
        if (player.forward.y < 0)
        {
            flip = -1;
        }
        gameObject.transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(player.forward, Vector2.right) * flip);
    }

    public void Attack()
    {
        ////Debug.Log("Attack");
        //flame.SetActive(true);
        //turnFlameOff = Time.time + duration;
        //nextAttack = Time.time + cooldown;

        //Debug.Log("Attack");
        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        float angleInc =  math.PI/18;
        float delay = .2f;
        for(int i = 0; i < amount; i++)
        {
            GameObject b = Instantiate(emberInstance);
            b.transform.SetParent(gameObject.transform, false);
            EmberInstance ei = b.GetComponent<EmberInstance>();
            ei.ember = this;
            ei.startTime = Time.time + delay * i;
            ei.goAwayTime = ei.startTime + 1.75f;

            ei.direction = rotate(direction, angleInc * i);
            b.transform.position = gameObject.transform.position;
            b.transform.localScale = Vector3.zero;
            //b.transform.eulerAngles += new Vector3(0, 0, angleInc * i);
        }

        nextAttack = Time.time + (cooldown);
    }
    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
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

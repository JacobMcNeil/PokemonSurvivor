using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Move;

public class Dragon : MonoBehaviour,Move
{
    public int level;
    [field: SerializeField]
    public string moveName { get; set; }

    [field: SerializeField]
    public string moveType { get; set; }
    [field: SerializeField]
    public PokeDex.PokeDexEntry pokemon { get; set; }
    public PartyController partyController { get; set; }


    public int totalDamage { get; set; }
    public float timeActive { get; set; }
    public float damage { get { return moveStats[0].Calc(baseDmgPoints); } private set { } }
    public float nextLeveldamage { get { return moveStats[0].Calc(baseDmgPoints + 1); } private set { } }
    public float cooldown { get { return moveStats[1].Calc(coolDownPoints); } private set { } }
    public float nextLevelcooldown { get { return moveStats[1].Calc(coolDownPoints + 1); } private set { } }
    public float size { get { return moveStats[2].Calc(sizePoints); } private set { } }
    public float nextLevelsize { get { return moveStats[2].Calc(sizePoints + 1); } private set { } }

    float duration;
    public List<moveStat> moveStats { get; set; } = new List<moveStat>();
    public int baseDmgPoints { get { return moveStats[0].statPoints; } set { } }
    public int coolDownPoints { get { return moveStats[1].statPoints; } set { } }
    public int sizePoints { get { return moveStats[2].statPoints; } set { } }


    float nextAttack = 0;
    public float tickRate = .25f;

    List<mob> hitMobs = new List<mob>();

    public GameObject dragonInstance;
    public Player player { get; set; }

    public PokeDex pokeDex { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        duration = 2f;
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Damage");
        moveStats[0].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(player.dragonFangAmount))) * ((pokemon.@base.Attack + player.might) *5 + 200f) / (50) * (x + 1)); };
        addMoveStat("Cooldown");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (1f * 100f / pokemon.@base.Speed) * ((float)(math.pow(.9, x))); };
        addMoveStat("Size");
        moveStats[2].Calc = (x) => { return (float)(math.pow(1.05, CalcMulti(0))) * (1.5f * pokemon.@base.Defense / 60f) * ((float)(math.pow(1.1, x))); };
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
        GameObject d = Instantiate(dragonInstance);
        if (hitMobs.Count > 0)
        {
            //d.transform.SetParent(hitMobs[0].gameObject.transform, false);
            d.GetComponent<DragonInstance>().trackingMob = hitMobs[UnityEngine.Random.Range(0, hitMobs.Count)];
        }
        d.transform.SetParent(gameObject.transform, false);
        Vector3 spawnlocation = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f));
        d.transform.position = spawnlocation.normalized * UnityEngine.Random.Range(2, 3) + player.transform.position;
        d.GetComponent<DragonInstance>().dragon = this;
        d.GetComponent<DragonInstance>().strikeTime = Time.time + duration;


        d.transform.localScale = new Vector3(0, 0, 1);
        nextAttack = Time.time + (cooldown);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mob m = collision.GetComponent<mob>();
        if (m != null)
        {
            hitMobs.Add(collision.gameObject.GetComponent<mob>());
        }
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

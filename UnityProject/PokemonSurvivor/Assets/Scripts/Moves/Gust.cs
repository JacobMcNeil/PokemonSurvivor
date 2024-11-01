using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Move;

public class Gust : MonoBehaviour, Move
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
    public int speedPoints { get { return moveStats[1].statPoints; } set { } }
    public int sizePoints { get { return moveStats[2].statPoints; } set { } }

    public PokeDex.PokeDexEntry pokemon { get; set; }

    public float damage { get { return moveStats[0].Calc(baseDmgPoints); } private set { } }
    public float nextLeveldamage { get { return moveStats[0].Calc(baseDmgPoints + 1); } private set { } }
    public float speed { get { return moveStats[1].Calc(speedPoints); } private set { } }
    public float nextLevelSpeed { get { return moveStats[1].Calc(speedPoints + 1); } private set { } }
    public float size { get { return moveStats[2].Calc(sizePoints); } private set { } }
    public float nextLevelsize { get { return moveStats[2].Calc(sizePoints + 1); } private set { } }

    float nextAttack = 0;
    float nextTick = 0;

    List<mob> hitMobs = new List<mob>();
    public PokeDex pokeDex { get; set; }
    public Player player { get; set; }

    public Vector2 moveDir;

    public float projectileSpeed;

    public float tickRate;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Damage");
        moveStats[0].Calc = (x) => { return Mathf.CeilToInt((float)(math.pow(1.05, CalcMulti(0))) * (pokemon.@base.Attack + player.might + 100f) / (50 + 100) * (x + 1)); };
        addMoveStat("Speed");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (4f * (90 + pokemon.@base.Speed) / (90 + 50)) * ((float)(math.pow(1.05, x))); };
        addMoveStat("Size");
        moveStats[2].Calc = (x) => { return (float)(math.pow(1.05, CalcMulti(0))) * (.9f * (pokemon.@base.Defense+350) / (40f + 350)) * ((float)(math.log10(x + 1)) + 1 + .05f * x); };
        partyController = GameObject.Find("Party").GetComponent<PartyController>();
        nextTick = Time.time;
        moveDir = new Vector2(.75f, .5f);
        moveDir.Normalize();
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
        if (Time.time > nextTick)
        {
            foreach (mob m in hitMobs)
            {
                for(int i = 0; i < 1 + Mathf.FloorToInt((Time.time - nextTick)/tickRate); i++)
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
                    //Debug.Log(m.health);
                }
            }
            nextTick = Time.time + tickRate;
            gameObject.transform.localScale = new Vector3(size, size, 1);
        }
        gameObject.transform.localPosition += (Vector3)moveDir * speed * Time.deltaTime;

        int screenSize = 17;
        if (gameObject.transform.localPosition.x > (screenSize - 3*transform.localScale.x) || gameObject.transform.localPosition.x < -1*(screenSize - 3 * transform.localScale.x))
        {
            moveDir = new Vector2(moveDir.x * -1, moveDir.y);
        }
        if (gameObject.transform.localPosition.y > (screenSize - 3 * transform.localScale.x) || gameObject.transform.localPosition.y < -1*(screenSize - 3 * transform.localScale.x))
        {
            moveDir = new Vector2(moveDir.x, moveDir.y * -1);
        }
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

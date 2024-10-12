using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static Move;

public class Ice : MonoBehaviour, Move
{
    public List<moveStat> moveStats { get; set; } = new List<moveStat>();
    public int level;
    [field: SerializeField]
    public string moveName { get; set; }

    public int totalDamage { get; set; }
    public float timeActive { get; set; }

    [field: SerializeField]
    public string moveType { get; set; }
    public int durationPoints { get { return moveStats[0].statPoints; } set { } }
    public int coolDownPoints { get { return moveStats[1].statPoints; } set { } }

    public float tickRate;
    public PokeDex.PokeDexEntry pokemon { get; set; }
    public PartyController partyController { get; set; }


    public float duration { get { return moveStats[0].Calc(durationPoints); } private set { } }
    public float nextLevelduration { get { return moveStats[0].Calc(durationPoints + 1); } private set { } }
    public float cooldown { get { return moveStats[1].Calc(coolDownPoints); } private set { } }
    public float nextLevelcooldown { get { return moveStats[1].Calc(coolDownPoints + 1); } private set { } }

    float nextAttack;

    public GameObject iceInstance;
    public Player player { get; set; }
    public PokeDex pokeDex { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        moveType = "ice";
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        addMoveStat("Duration");
        moveStats[0].Calc = (x) => { return ((float)(math.pow(1.05, CalcMulti(player.neverMeltAmount))) * 3f * pokemon.@base.Defense / 60) * ((float)(math.pow(1.1, x))); };
        addMoveStat("Cooldown");
        moveStats[1].Calc = (x) => { return (float)(math.pow(.95, CalcMulti(0))) * (1 * 120f / pokemon.@base.Speed) * ((float)(math.pow(.9, x))); };
        player = GameObject.Find("Player").GetComponent<Player>();
        partyController = GameObject.Find("Party").GetComponent<PartyController>();
        nextAttack = 0f;
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

    float currentAngle = 0;
    float angleInc = 2 * math.PI / 16f;

    public void Attack()
    {
        GameObject i = Instantiate(iceInstance);
        i.GetComponent<IceInstance>().ice = this;
        i.GetComponent<IceInstance>().goAwayTime = Time.time + .1f;
        i.transform.SetParent(gameObject.transform, false);

        i.transform.eulerAngles = new Vector3(0,0,currentAngle * 180f/math.PI);
        currentAngle += angleInc;

        nextAttack = Time.time + (cooldown);

    }
}

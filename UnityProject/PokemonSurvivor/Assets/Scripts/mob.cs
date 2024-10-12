using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class mob : MonoBehaviour
{
    public float currentHp;
    float maxHp;
    public float hp
    {
        get{return CalcHP(level, pokemon.@base.HP);} 
        private set { }
    }
    public float attack
    {
        get { return CalcStat(level, pokemon.@base.Attack); }
        private set { }
    }
    public float spAttack
    {
        get { return CalcStat(level, pokemon.@base.SpAttack); }
        private set { }
    }
    public float defence
    {
        get { return CalcStat(level, pokemon.@base.Defense); }
        private set { }
    }
    public float spDefence
    {
        get { return CalcStat(level, pokemon.@base.SpDefense); }
        private set { }
    }

    public float speed
    {
        get { return CalcStat(level, pokemon.@base.Speed); }
        private set { }
    }
    bool _isBoss = false;
    public bool isBoss { get { return _isBoss; }
        set
        {
            if (value)
            {
                level *= 2;
                healthBar.transform.parent.gameObject.SetActive(true);
            }
            _isBoss = value;
        }
    }
    bool _isFrozen;
    public bool isFrozen
    {
        get { return _isFrozen; }
        set
        {
            if (value)
            {
                spriteRenderer.color = UnityEngine.Color.blue;
            }
            else
            {
                spriteRenderer.color = UnityEngine.Color.white;
            }
            _isFrozen = value;
        }
    }
    public float unFreezeTime;
    public float coolDown;

    public int pokemonId;
    public int level;
    public PokeDex.PokeDexEntry pokemon;
    public PokeDex pokeDex;

    public bool canAttack = true;
    bool oldCanAttack = true;
    float lastAttackTime;

    public GameObject player;
    public Player playerScript;
    public Rigidbody2D playerRb;
    public Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    public GameObject EXP;
    public GameObject DamageNumber;
    public List<GameObject> bossDrops;

    public Image healthBar;
    public float levelScale;
    public float distanceFromPlayer;


    // Start is called before the first frame update
    void Awake()
    {
        PolygonCollider2D p =  gameObject.AddComponent<PolygonCollider2D>();
        var myPoints = p.points;
        // do stuff with myPoints array
        for (int i =0; i < p.points.Count(); i ++)
        {
            //Debug.Log("points");
            myPoints[i] = myPoints[i] * .8f; 
        }
        p.points = myPoints;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
        playerRb = player.GetComponent<Rigidbody2D>();
        pokeDex = GameObject.Find("PokeDex").GetComponent<PokeDex>();
        level = playerRb.gameObject.GetComponent<Player>().level;
        distanceFromPlayer = 10;
        //Debug.Log("max hp" + currentHp);
        //Debug.Log("LEvel" + level);
        //Debug.Log("dmg" + (attack + spAttack) * .1f);
        //Debug.Log("hp"+ hp);
        //Debug.Log("attack" + attack);
        //Debug.Log(spAttack);
        //Debug.Log("Defence" + defence);
        //Debug.Log(spDefence);
        //Debug.Log(speed);

    }
    public void scaleLevel(float scale)
    {
        level = Mathf.CeilToInt(level/(scale*1.5f));
    }

    public void SetPokemonID(int iD)
    {
        pokemonId = iD;
        pokemon = pokeDex.pokeDex[pokemonId - 1];
        if (pokemon.evolution.next != null && pokemon.evolution.next.Count > 0)
        {
            //Debug.Log(pokemon.evolution.next[0][0]);
            long nextlevel;

            Int64.TryParse(pokemon.evolution.next[0][1], out nextlevel);
            long evoID;
            Int64.TryParse(pokemon.evolution.next[0][0], out evoID);
            if (nextlevel == 0)
            {
                nextlevel = 35;
            }
            if (level >= nextlevel)
            {
                pokemon = pokeDex.pokeDex[(int)(evoID - 1)];
                pokemonId = (int)(evoID);
            }
        }
        if (pokemon.evolution.next != null && pokemon.evolution.next.Count > 0)
        {
            //Debug.Log(pokemon.evolution.next[0][0]);
            long nextlevel;

            Int64.TryParse(pokemon.evolution.next[0][1], out nextlevel);
            long evoID;
            Int64.TryParse(pokemon.evolution.next[0][0], out evoID);
            if (nextlevel == 0)
            {
                nextlevel = 35;
            }
            if (level >= nextlevel)
            {
                pokemon = pokeDex.pokeDex[(int)(evoID - 1)];
                pokemonId = (int)(evoID);
            }
        }
        maxHp = (hp + defence + spDefence) / 3;
        currentHp = maxHp;
        SetImage(pokemonId);
    }

    public void SetImage(int pokemonId)
    {
        string spriteFileString = pokemonId.ToString();

        //while (spriteFileString.Count() < 3)
        //{
        //    spriteFileString = "0" + spriteFileString;
        //}
        Sprite sprite = Resources.Load<Sprite>("sprites/" + spriteFileString);
        spriteRenderer.sprite = sprite;
        if (gameObject.GetComponent<PolygonCollider2D>() != null)
        {
            Destroy(gameObject.GetComponent<PolygonCollider2D>());
        }
        PolygonCollider2D p = gameObject.AddComponent<PolygonCollider2D>();
        var myPoints = p.points;
        // do stuff with myPoints array
        for (int i = 0; i < p.points.Count(); i++)
        {
            myPoints[i] = myPoints[i] * .8f;
        }
        p.points = myPoints;
        rb = GetComponent<Rigidbody2D>();
    }
    public Vector2 dir;
    // Update is called once per frame
    void Update()
    {
        dir = playerRb.position - rb.position;

        if (!isFrozen)
        {
            rb.velocity = dir.normalized * (speed * .003f + .5f);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (Time.time > unFreezeTime)
        {
            isFrozen = false;
        }


        if (!canAttack && oldCanAttack)
        {
            lastAttackTime = Time.time;
        }
        if(Time.time-lastAttackTime > coolDown)
        {
            canAttack = true;
        }
        oldCanAttack = canAttack;

        distanceFromPlayer = (playerRb.position - rb.position).magnitude;
        //Debug.Log(playerScript.closestMob.distanceFromPlayer);
        if (playerScript.closestMob.distanceFromPlayer > distanceFromPlayer)
        {
            playerScript.closestMob = this;
            //Debug.Log(playerScript.closestMob.distanceFromPlayer);
        }
        if (currentHp <= 0)
        {
            Instantiate(EXP).transform.position = gameObject.transform.position;
            if (isBoss)
            {
                float currentAngle = 0;
                float angleInc = 2 * math.PI / bossDrops.Count;
                foreach(GameObject g in bossDrops)
                {
                    Vector3 test = new Vector3(math.cos(currentAngle), math.sin(currentAngle), 0);
                    currentAngle += angleInc;
                    
                    Instantiate(g).transform.position = gameObject.transform.position + test;
                }
            }
            distanceFromPlayer = 10;
            Destroy(gameObject.transform.parent.gameObject);
        }

    }
    public void Damage(int amount)
    {
        currentHp -= amount;
        GameObject damageNumber =  Instantiate(DamageNumber);
        damageNumber.transform.position = gameObject.transform.position;
        damageNumber.GetComponent<TextMeshPro>().text = amount.ToString();
        healthBar.transform.localScale = new Vector3(currentHp / maxHp, 1, 1);
    }
    public int CalcStat(int level, long baseStat)
    {
        //(floor(0.01 x (2 x Base + IV + floor(0.25 x EV)) x Level) + 5)
        return Mathf.FloorToInt(.01f * (2 * baseStat + 15) * (level) + 5);
    }
    public int CalcHP(int level, long baseStat)
    {
        //floor(0.01 x (2 x Base + IV + floor(0.25 x EV)) x Level) + Level + 10
        int bossHPMult = 1;
        if (isBoss)
        {
            bossHPMult = 30;
        }
        return Mathf.FloorToInt(.01f * (2 * baseStat + 15) * level + level + 10) * bossHPMult;
    }
}

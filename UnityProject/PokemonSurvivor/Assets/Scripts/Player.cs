using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public int currentEXP;
    public int nextLevelEXP;
    public int lastLevelEXP = 0;
    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;

    public int level = 0;
    public float healthRegen;
    float nextRegen;
    public int might;

    public Vector2 forward = Vector2.right;
    public Vector2 lastForward = Vector2.right;

    public GameObject DamageNumber;
    public Image healthBar;
    public Image EXPBar;
    public SpriteRenderer playerSprite;
    public GameObject levelUpWindow;
    public GameObject deathWindow;
    public GameObject trainerCardWindow;
    public GameObject pickUpCollider;

    public Revenge revenge;
    public IronDefence ironDefence;

    public mob closestMob;


    public VariableJoystick variableJoystick;

    public int fireStoneAmount;
    public int thunderStoneAmount;
    public int leafStoneAmount;
    public int waterStoneAmount;
    public int dragonFangAmount;
    public int toxicBarbAmount;
    public int hardStoneAmount;
    public int neverMeltAmount;
    public int tradeCableAmount;
    public int sootheBellAmount;
    public int everStoneAmount;
    public int commonBoostAmount;
    public int rareBoostAmount;
    int _itemFinderAmount;
    public int itemFinderAmount { get { return _itemFinderAmount; } set { _itemFinderAmount = value; pickUpCollider.transform.localScale *= 1.5f; } }
    void Start()
    {
        healthRegen = 0;
        nextRegen = Time.time;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        UpdateEXPBar();
        levelUpWindow.SetActive(true);
        variableJoystick.SetMode(JoystickType.Floating);
        variableJoystick.gameObject.SetActive(true);
        might = 0;
        fireStoneAmount = 0;
        closestMob = new mob();
        closestMob.distanceFromPlayer = 10;
    }

    void Update()
    {
        float h;
        float v;
        Vector2 tempVect;
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            h = variableJoystick.Horizontal;
            v = variableJoystick.Vertical;

            tempVect = new Vector2(h, v);
            if (h != 0 && v != 0)
            {
                forward = tempVect;
            }
        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            tempVect = new Vector2(h, v);
            if (!(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0))
            {
                forward = tempVect;
            }
        }
        rb.velocity = tempVect.normalized * speed;

        List<Collider2D> results = new List<Collider2D>();
        playerCollider.OverlapCollider(new ContactFilter2D(), results);
        foreach(Collider2D c in results)
        {
            mob m = c.gameObject.GetComponent<mob>();
            if (m != null)
            {
                if (m.canAttack)
                {
                    if (!m.isFrozen)
                    {
                        int damageAmount = Mathf.CeilToInt((m.attack + m.spAttack) * .1f);
                        //Debug.Log(damageAmount);
                        if (revenge != null)
                        {
                            damageAmount -= (int)revenge.damageReduction;
                        }
                        //Debug.Log(damageAmount);
                        if (damageAmount > 0)
                        {
                            if (ironDefence != null)
                            {
                                if (ironDefence.currentStacks == 0)
                                {
                                    SetHp((int)health - damageAmount);
                                }
                                else
                                {
                                    ironDefence.currentStacks--;
                                }
                            }
                            else
                            {
                                SetHp((int)health - damageAmount);
                            }
                        }
                    }
                    m.canAttack = false;
                    if (revenge != null)
                    {
                        revenge.activate = true;
                    }
                }
            }
        }
        //Debug.Log(results.Count);
        //Debug.Log(health);
        if(Time.time > nextRegen)
        {
            SetHp((int)(health + healthRegen));
            nextRegen = Time.time + 1;
        }
        if (ironDefence != null)
        {
            Debug.Log(ironDefence.currentStacks);
            playerSprite.color = new Color((1f*255-ironDefence.currentStacks)/255, (1f * 255 - ironDefence.currentStacks) / 255, (1f * 255 - ironDefence.currentStacks) / 255);
        }
    }
    public void GainEXP(int amout)
    {
        currentEXP += amout;
        UpdateEXPBar();
        if (currentEXP >= nextLevelEXP)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level += 1;
        lastLevelEXP = nextLevelEXP;
        currentEXP = 0;
        if (level < 20)
        {
            nextLevelEXP += 3;
        }
        else if (level < 40)
        {
            nextLevelEXP += 5;
        }
        else if (level >= 40)
        {
            nextLevelEXP += 7;
        }
        UpdateEXPBar();
        levelUpWindow.SetActive(true);
    }
    public void SetHp(int value)
    {
        if (value > maxHealth)
        {
            value = (int)maxHealth;
        }
        if(value != health)
        {
            int amount = Mathf.FloorToInt(health - value);
            Color c;
            if (value > health)
            {
                c = Color.green;
            }
            else
            {
                c = Color.red;
            }
            health = value;
            GameObject damageNumber = Instantiate(DamageNumber);
            damageNumber.transform.position = gameObject.transform.position;
            damageNumber.GetComponent<TextMeshPro>().text = math.abs(amount).ToString();
            damageNumber.GetComponent<TextMeshPro>().color = c;
        }

        if (health <= 0)
        {
            deathWindow.SetActive(true);
        }
        healthBar.transform.localScale = new Vector3(health / maxHealth, 1, 1);
    }
    public void UpdateEXPBar()
    {
        EXPBar.transform.localScale = new Vector3(currentEXP * 1f/nextLevelEXP, 1, 1);
    }
}

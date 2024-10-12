using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicInstance : MonoBehaviour
{
    //float nextAttack = 0;

    public Toxic toxic;

    List<mob> hitMobs = new List<mob>();
    float nextTick;
    public float goAwayTime;
    //public Player player;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Player").GetComponent<Player>();
        nextTick = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * toxic.size);

        if (Time.time > nextTick)
        {
            foreach (mob m in hitMobs)
            {
                for (int i = 0; i < 1 + Mathf.FloorToInt((Time.time - nextTick) / toxic.tickRate); i++)
                {
                    if (toxic.damage < m.currentHp)
                    {
                        toxic.totalDamage += (int)toxic.damage;
                    }
                    else
                    {
                        toxic.totalDamage += (int)m.currentHp;
                    }
                    m.Damage((int)toxic.damage);
                    //Debug.Log(m.health);
                }
            }
            nextTick = Time.time + toxic.tickRate;
        }
        if (Time.time > goAwayTime)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        mob m = collision.GetComponent<mob>();
        if (m != null)
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

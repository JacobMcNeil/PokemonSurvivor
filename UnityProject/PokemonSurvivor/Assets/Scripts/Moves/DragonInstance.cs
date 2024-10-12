using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonInstance : MonoBehaviour
{
    //float nextAttack = 0;

    public Dragon dragon;

    public mob trackingMob;

    List<mob> hitMobs = new List<mob>();
    float nextTick = 0;
    public float strikeTime;
    float startTime;
    //public Player player;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        //player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * toxic.size);
        gameObject.transform.localScale = Vector3.one * dragon.size *  ((Time.time - startTime) / (strikeTime - startTime));
        if (Time.time > strikeTime)
        {
            foreach (mob m in hitMobs)
            {
                if (dragon.damage < m.currentHp)
                {
                    dragon.totalDamage += (int)dragon.damage;
                }
                else
                {
                    dragon.totalDamage += (int)m.currentHp;
                }
                m.Damage((int)dragon.damage);
                //Debug.Log(m.health);
            }
            Destroy(gameObject);
        }
        if (trackingMob != null)
        {
            transform.position = new Vector3(trackingMob.transform.position.x, trackingMob.transform.position.y, 0);
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

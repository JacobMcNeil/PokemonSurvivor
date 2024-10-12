using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberInstance : MonoBehaviour
{
    //float nextAttack = 0;

    public Ember ember;

    List<mob> hitMobs = new List<mob>();
    float nextTick;
    public float goAwayTime;
    public Vector2 direction;
    float projectileSpeed;
    public float startTime;
    //public Player player;
    // Start is called before the first frame update
    void Start()
    {
        projectileSpeed = 3f;
        //player = GameObject.Find("Player").GetComponent<Player>();
        nextTick = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime)
        {
            gameObject.transform.SetParent(null, true);
            //gameObject.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * toxic.size);
            gameObject.transform.position += (Vector3)direction * projectileSpeed * Time.deltaTime;
            if (gameObject.transform.localScale.x < 1.5)
            {
                gameObject.transform.localScale += Vector3.one * Time.deltaTime;
            }

            if (Time.time > nextTick)
            {
                foreach (mob m in hitMobs)
                {
                    for (int i = 0; i < 1 + Mathf.FloorToInt((Time.time - nextTick) / ember.tickRate); i++)
                    {
                        if (ember.damage < m.currentHp)
                        {
                            ember.totalDamage += (int)ember.damage;
                        }
                        else
                        {

                            ember.totalDamage += (int)m.currentHp;
                        }
                        m.Damage((int)ember.damage);
                        //Debug.Log(m.health);
                    }
                }
                nextTick = Time.time + ember.tickRate;
            }
            if (Time.time > goAwayTime)
            {
                Destroy(gameObject);
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashInstance : MonoBehaviour
{

    public Slash slash;

    float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        destroyTime = Time.time + .2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > destroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mob m = collision.GetComponent<mob>();
        if (m != null)
        {
            if (slash.damage < m.currentHp)
            {
                slash.totalDamage += (int)slash.damage;
            }
            else
            {
                slash.totalDamage += (int)m.currentHp;
            }
            m.Damage((int)slash.damage);
        }
        //Debug.Log("add");
    }
}

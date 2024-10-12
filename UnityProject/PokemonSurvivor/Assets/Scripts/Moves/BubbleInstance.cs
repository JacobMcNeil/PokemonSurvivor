using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleInstance : MonoBehaviour
{

    //float nextAttack = 0;

    public Bubble bubble;

    //public Player player;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * bubble.speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mob m = collision.GetComponent<mob>();
        if (m != null)
        {
            if (bubble.damage < m.currentHp)
            {
                bubble.totalDamage += (int)bubble.damage;
            }
            else
            {
                bubble.totalDamage += (int)m.currentHp;
            }
            m.Damage((int)bubble.damage);
            Destroy(gameObject);
        }
        //Debug.Log("add");
    }
}

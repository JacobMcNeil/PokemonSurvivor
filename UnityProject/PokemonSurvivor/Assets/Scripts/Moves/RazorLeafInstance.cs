using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorLeafInstance: MonoBehaviour
{

    public float speed;

    float nextAttack;

    public RazorLeaf bulbasaur;

    public Player player;
    public float goAwayTime;
    public float pierce;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        speed = 17;
        nextAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += transform.right * speed * Time.deltaTime;
        if (Time.time > goAwayTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mob m = collision.GetComponent<mob>();
        if (m != null && pierce >= 0)
        {
            if (bulbasaur.damage < m.currentHp)
            {
                bulbasaur.totalDamage += (int)bulbasaur.damage;
            }
            else
            {
                bulbasaur.totalDamage += (int)m.currentHp;
            }
            m.Damage((int)bulbasaur.damage);
            if (pierce <= 0)
            {
                Destroy(gameObject);
            }
            pierce -= 1;
        }
        if (collision.GetComponent<LostZone>())
        {
            Destroy(gameObject);
        }

    }
}

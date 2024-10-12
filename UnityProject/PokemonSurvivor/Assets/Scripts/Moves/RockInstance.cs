using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockInstance : MonoBehaviour
{
    // Start is called before the first frame update
    public Rock rock;

    List<mob> hitMobs = new List<mob>();
    public float goAwayTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("rock");
            hitMobs.Add(collision.gameObject.GetComponent<mob>());
            if (rock.damage < m.currentHp)
            {
                rock.totalDamage += (int)rock.damage;
            }
            else
            {
                rock.totalDamage += (int)m.currentHp;
            }
            m.Damage((int)rock.damage);

        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceInstance : MonoBehaviour
{
    // Start is called before the first frame update
    public Ice ice;

    List<mob> hitMobs = new List<mob>();
    public float goAwayTime;

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
            Debug.Log("freeze");
            hitMobs.Add(collision.gameObject.GetComponent<mob>());
            m.isFrozen = true;
            m.unFreezeTime = Time.time + ice.duration;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItems : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EXP e = collision.transform.GetComponent<EXP>();
        if (e != null)
        {
            e.collect = true;
        }
        Item i = collision.transform.GetComponent<Item>();
        if (i != null)
        {
            i.collect = true;
        }
        Magnet m = collision.transform.GetComponent<Magnet>();
        if (m != null)
        {
            m.collect = true;
        }
        Pokeball p = collision.transform.GetComponent<Pokeball>();
        if (p != null)
        {
            p.collect = true;
        }
        Debug.Log("add");
    }
}

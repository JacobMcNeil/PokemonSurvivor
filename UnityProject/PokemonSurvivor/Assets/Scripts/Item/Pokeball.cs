using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    public Player player;
    public bool collect = false;
    float collectSpeed = 2f;
    public GameObject trainerCardWindow;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        trainerCardWindow = player.trainerCardWindow;
    }

    // Update is called once per frame
    void Update()
    {
        if (collect && Time.timeScale != 0)
        {
            Vector3 dir = player.transform.position - gameObject.transform.position;
            gameObject.transform.position += ((dir).normalized * Time.deltaTime * collectSpeed);
            collectSpeed += .02f;
        }
        if ((player.transform.position - gameObject.transform.position).magnitude < .1f)
        {
            Destroy(gameObject);
            trainerCardWindow.SetActive(true);
        }
    }
}

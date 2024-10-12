using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public Player player;
    public GameObject berry;
    float nextSpawn;
    float interval = 45;
    // Start is called before the first frame update
    void Start()
    {
        nextSpawn = Time.time + interval;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            Vector3 spawnlocation = new Vector3(UnityEngine.Random.Range(-7.0f, 7.0f), UnityEngine.Random.Range(-7.0f, 7.0f));

            //Debug.Log("spawn");
            GameObject newItem = Instantiate(berry);
            //newMob.GetComponentInChildren<mob>().SetPokemonID(waves[currentWave % waves.Count][UnityEngine.Random.Range(0, waves[currentWave % waves.Count].Count)]);
            newItem.transform.position = spawnlocation.normalized * 7 + player.transform.position;

            nextSpawn = Time.time + interval;
        }
    }
}

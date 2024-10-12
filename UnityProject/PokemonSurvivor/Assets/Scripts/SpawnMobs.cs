using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnMobs : MonoBehaviour
{

    public float spawnRate = 0f;
    float lastSpawn;

    List<List<int>> waves = new List<List<int>>();
    int currentWave = 0;
    float waveDuration = 60;
    float nextWave;
    float nextBoss;
    public float bossDelay;
    float nextFinalBoss;
    List<int> finalBosses = new List<int>();
    public float finalBossDelay;
    float randomScale = 1f;
    public int stageLength;


    public GameObject mob;
    public GameObject player;

    public Timer timer;
    List<float> waveScaleArray = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        waveScaleArray.Add(.7f);
        waveScaleArray.Add(1f);
        waveScaleArray.Add(2f);
        finalBosses.Add(150);
        finalBosses.Add(251);
        finalBosses.Add(386);
        finalBosses.Add(384);
        finalBosses.Add(890);
        lastSpawn = timer.timerTime;
        nextWave = timer.timerTime + waveDuration;
        nextBoss = timer.timerTime + 45;
        nextFinalBoss = timer.timerTime + stageLength;
        waves.Add(new List<int>());
        waves[0].Add(19);
        waves[0].Add(16);
        waves[0].Add(21);
        waves.Add(new List<int>());
        waves[1].Add(10);
        waves[1].Add(13);
        waves[1].Add(25);
        waves.Add(new List<int>());
        waves[2].Add(74);
        waves[2].Add(41);
        waves[2].Add(95);
    }
    public void ResetTimers()
    {
        lastSpawn = timer.timerTime;
        nextWave = timer.timerTime + waveDuration;
        nextBoss = timer.timerTime + 45;
        nextFinalBoss = timer.timerTime + 600;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.timerTime < stageLength)
        {
            if (timer.timerTime - lastSpawn > (spawnRate / randomScale) && Time.timeScale != 0)
            {
                Vector3 spawnlocation = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f));

                lastSpawn = timer.timerTime;
                //Debug.Log("spawn");
                GameObject newMob = Instantiate(mob);
                newMob.GetComponentInChildren<mob>().scaleLevel(randomScale);
                newMob.GetComponentInChildren<mob>().SetPokemonID(waves[currentWave % waves.Count][UnityEngine.Random.Range(0, waves[currentWave % waves.Count].Count)]);
                newMob.transform.position = spawnlocation.normalized * 7 + player.transform.position;
            }
            if (timer.timerTime > nextBoss)
            {
                Vector3 spawnlocation = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f));

                //Debug.Log("spawn");
                GameObject newMob = Instantiate(mob);
                newMob.transform.localScale *= 2;
                newMob.GetComponentInChildren<mob>().isBoss = true;
                newMob.GetComponentInChildren<mob>().SetPokemonID(waves[currentWave % waves.Count][UnityEngine.Random.Range(0, waves[currentWave % waves.Count].Count)]);
                newMob.transform.position = spawnlocation.normalized * 7 + player.transform.position;
                nextBoss = timer.timerTime + bossDelay;
            }
            if (timer.timerTime > nextWave)
            {
                randomScale = waveScaleArray[UnityEngine.Random.Range(0, waveScaleArray.Count)];
                currentWave += 1;
                nextWave = timer.timerTime + waveDuration;
                spawnRate = spawnRate - .02f;
            }
        }
        if(timer.timerTime > nextFinalBoss )
        {
            Vector3 spawnlocation = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f));

            //Debug.Log("spawn");
            GameObject newMob = Instantiate(mob);
            newMob.GetComponentInChildren<mob>().scaleLevel(.2f);
            newMob.transform.localScale *= 3;
            newMob.GetComponentInChildren<mob>().isBoss = true;
            newMob.GetComponentInChildren<mob>().SetPokemonID(finalBosses[(Mathf.FloorToInt((timer.timerTime - stageLength) /bossDelay))%finalBosses.Count]);
            newMob.transform.position = spawnlocation.normalized * 7 + player.transform.position;
            nextFinalBoss = timer.timerTime + finalBossDelay;
        }
    }
    private void FixedUpdate()
    {
        spawnRate = spawnRate * .9999f;
    }
}

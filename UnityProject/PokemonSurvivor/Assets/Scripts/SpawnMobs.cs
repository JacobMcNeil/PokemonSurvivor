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
    public float stageLength;
    public LevelUpWindow levelUpWindow;


    public GameObject mob;
    public GameObject player;

    public Timer timer;
    List<float> waveScaleArray = new List<float>();

    public class stage
    {
        public string name;
        public float stageLength;
        public float waveDuration;
        public List<List<int>> waves;
        public List<int> finalBosses;
    }

    List<stage> stages = new List<stage>();
    public stage currentStage;
    // Start is called before the first frame update
    void Start()
    {

        stage s = new stage();
        s.name = "Brock";
        s.stageLength = 300;
        s.waveDuration = 60;
        s.waves = new List<List<int>>();
        s.waves.Add(new List<int>());
        s.waves[0].Add(19);
        s.waves[0].Add(16);
        s.waves.Add(new List<int>());
        s.waves[1].Add(21);
        s.waves[1].Add(32);
        s.waves[1].Add(29);
        s.waves[1].Add(56);
        s.waves.Add(new List<int>());
        s.waves[2].Add(10);
        s.waves[0].Add(16);
        s.waves[2].Add(13);
        s.waves[2].Add(172);
        s.finalBosses = new List<int>();
        s.finalBosses.Add(208);
        s.finalBosses.Add(464);
        s.finalBosses.Add(38);
        s.finalBosses.Add(141);
        s.finalBosses.Add(142);
        s.finalBosses.Add(76);
        stages.Add(s);

        stage s1 = new stage();
        s1.name = "Misty";
        s1.stageLength = 600;
        s1.waveDuration = 60;
        s1.waves = new List<List<int>>();
        s1.waves.Add(new List<int>());
        s1.waves[0].Add(19);
        s1.waves[0].Add(16);
        s1.waves[0].Add(21);
        s1.waves[0].Add(27);
        s1.waves[0].Add(56);
        s1.waves[0].Add(39);
        s1.waves.Add(new List<int>());
        s1.waves[1].Add(74);
        s1.waves[1].Add(173);
        s1.waves[1].Add(46);
        s1.waves[1].Add(27);
        s1.waves[1].Add(41);
        s1.waves.Add(new List<int>());
        s1.waves[2].Add(23);
        s1.waves[2].Add(43);
        s1.waves[2].Add(69);
        s1.waves[2].Add(63);
        s1.waves[2].Add(48);
        s1.finalBosses = new List<int>();
        s1.finalBosses.Add(121);
        s1.finalBosses.Add(55);
        s1.finalBosses.Add(119);
        s1.finalBosses.Add(230);
        s1.finalBosses.Add(91);
        s1.finalBosses.Add(134);
        stages.Add(s1);

        stage s2 = new stage();
        s2.name = "Lt.Surge";
        s2.stageLength = 600;
        s2.waveDuration = 60;
        s2.waves = new List<List<int>>();
        s2.waves.Add(new List<int>());
        s2.waves[0].Add(43);
        s2.waves[0].Add(69);
        s2.waves[0].Add(56);
        s2.waves[0].Add(52);
        s2.waves[0].Add(39);
        s2.waves[0].Add(16);
        s2.waves.Add(new List<int>());
        s2.waves[1].Add(50);
        s2.waves[1].Add(50);
        s2.waves[1].Add(50);
        s2.waves[1].Add(50);
        s2.waves[1].Add(50);
        s2.waves[1].Add(51);
        s2.waves.Add(new List<int>());
        s2.waves[2].Add(23);
        s2.waves[2].Add(27);
        s2.waves[2].Add(21);
        s2.waves[2].Add(96);
        s2.waves[2].Add(129);
        s2.finalBosses = new List<int>();
        s2.finalBosses.Add(101);
        s2.finalBosses.Add(462);
        s2.finalBosses.Add(466);
        s2.finalBosses.Add(135);
        s2.finalBosses.Add(171);
        s2.finalBosses.Add(26);
        stages.Add(s2);

        stage s3 = new stage();
        s3.name = "Erika";
        s3.stageLength = 600;
        s3.waveDuration = 60;
        s3.waves = new List<List<int>>();
        s3.waves.Add(new List<int>());
        s3.waves[0].Add(41);
        s3.waves[0].Add(74);
        s3.waves[0].Add(66);
        s3.waves[0].Add(95);
        s3.waves.Add(new List<int>());
        s3.waves[1].Add(92);
        s3.waves[1].Add(104);
        s3.waves.Add(new List<int>());
        s3.waves[2].Add(100);
        s3.waves[2].Add(81);
        s3.waves[2].Add(129);
        s3.waves[2].Add(37);
        s3.waves[2].Add(58);
        s3.waves[2].Add(52);
        s3.waves[2].Add(19);
        s3.finalBosses = new List<int>();
        s3.finalBosses.Add(465);
        s3.finalBosses.Add(71);
        s3.finalBosses.Add(45);
        s3.finalBosses.Add(470);
        s3.finalBosses.Add(182);
        s3.finalBosses.Add(3);
        stages.Add(s3);

        stage s4 = new stage();
        s4.name = "Koga";
        s4.stageLength = 600;
        s4.waveDuration = 60;
        s4.waves = new List<List<int>>();
        s4.waves.Add(new List<int>());
        s4.waves[0].Add(43);
        s4.waves[0].Add(48);
        s4.waves[0].Add(83);
        s4.waves[0].Add(74);
        s4.waves[0].Add(118);
        s4.waves[0].Add(143);
        s4.waves.Add(new List<int>());
        s4.waves[1].Add(21);
        s4.waves[1].Add(84);
        s4.waves[1].Add(77);
        s4.waves.Add(new List<int>());
        s4.waves[2].Add(102);
        s4.waves[2].Add(111);
        s4.waves[2].Add(123);
        s4.waves[2].Add(440);
        s4.waves[2].Add(114);
        s4.waves[2].Add(115);
        s4.waves[2].Add(104);
        s4.waves[2].Add(128);
        s4.finalBosses = new List<int>();
        s4.finalBosses.Add(110);
        s4.finalBosses.Add(169);
        s4.finalBosses.Add(73);
        s4.finalBosses.Add(89);
        s4.finalBosses.Add(49);
        s4.finalBosses.Add(34);
        stages.Add(s4);

        waveScaleArray.Add(.7f);
        waveScaleArray.Add(1f);
        waveScaleArray.Add(2f);
        //finalBosses.Add(150);
        //finalBosses.Add(251);
        //finalBosses.Add(386);
        //finalBosses.Add(384);
        //finalBosses.Add(890);
        int stageIndex = Settings.stageIndex;//UnityEngine.Random.Range(0, stages.Count);
        currentStage = stages[stageIndex];
        waves = currentStage.waves;
        finalBosses = currentStage.finalBosses;
        waveDuration = currentStage.waveDuration;
        stageLength = currentStage.stageLength;
        levelUpWindow.wavePokemon = currentStage.waves[0];



        lastSpawn = timer.timerTime;
        nextWave = timer.timerTime + waveDuration;
        nextBoss = timer.timerTime + 45;
        nextFinalBoss = timer.timerTime + stageLength;
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
                levelUpWindow.wavePokemon = currentStage.waves[currentWave%currentStage.waves.Count];
                spawnRate = spawnRate - .02f;
            }
        }
        if(timer.timerTime > nextFinalBoss )
        {
            if ((Mathf.FloorToInt((timer.timerTime - stageLength) / finalBossDelay)) < finalBosses.Count)
            {
                Vector3 spawnlocation = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f));

                //Debug.Log("spawn");
                GameObject newMob = Instantiate(mob);
                newMob.GetComponentInChildren<mob>().scaleLevel(.5f);
                newMob.transform.localScale *= 3;
                newMob.GetComponentInChildren<mob>().isBoss = true;
                newMob.GetComponentInChildren<mob>().SetPokemonID(finalBosses[(Mathf.FloorToInt((timer.timerTime - stageLength) / finalBossDelay)) % finalBosses.Count]);
                newMob.transform.position = spawnlocation.normalized * 7 + player.transform.position;
                nextFinalBoss = timer.timerTime + finalBossDelay;
            }
            else 
            {
                Vector3 spawnlocation = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f));

                //Debug.Log("spawn");
                GameObject newMob = Instantiate(mob);
                newMob.GetComponentInChildren<mob>().scaleLevel(.2f);
                newMob.transform.localScale *= 3;
                newMob.GetComponentInChildren<mob>().isBoss = true;
                newMob.GetComponentInChildren<mob>().SetPokemonID(150);
                newMob.transform.position = spawnlocation.normalized * 7 + player.transform.position;
                nextFinalBoss = timer.timerTime + finalBossDelay;
                if (Settings.highestStageBeat < Settings.stageIndex)
                {
                    Settings.highestStageBeat = Settings.stageIndex;

                    Settings.saveHighestStageBeat();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        spawnRate = spawnRate * .9999f;
    }
}

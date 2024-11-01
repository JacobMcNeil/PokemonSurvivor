using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathWindow : MonoBehaviour
{
    [SerializeField]
    Button keepLevels_B;
    [SerializeField]
    Button restart_B;
    [SerializeField]
    Button MainMenu_B;

    public Player player;
    public SpawnMobs spawnMobs;
    public Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        keepLevels_B.onClick.AddListener(KeepLevels);
        restart_B.onClick.AddListener(Restart);
        MainMenu_B.onClick.AddListener(GotoMainMenu);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameObject.SetActive(false);
    }

    private void KeepLevels()
    {
        player.transform.position = Vector3.zero;
        player.SetHp(100);
        player.currentEXP = 0;
        player.lastLevelEXP = 0;
        player.nextLevelEXP = 10;
        player.UpdateEXPBar();
        spawnMobs.spawnRate = 1;
        timer.lastReset = Time.time;
        spawnMobs.ResetTimers();

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Mob"))
        {
            Destroy(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Item"))
        {
            Destroy(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("AttackInstance"))
        {
            Destroy(g);
        }
        gameObject.SetActive(false);

        //List<GameObject> old = GameObject.FindGameObjectsWithTag("Pokemon").ToList();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        //List<GameObject> newG = GameObject.FindGameObjectsWithTag("Pokemon").ToList();

        //for (int i = 0; i < newG.Count; i++)
        //{
        //    Pokemon newp = newG[i].GetComponent<Pokemon>();
        //    Pokemon oldp = old[i].GetComponent<Pokemon>();
        //    Debug.Log(oldp.pokemonName);
        //    Debug.Log(oldp.baseDmg);
        //    newp.baseDmg = oldp.baseDmg;
        //}


    }

    void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

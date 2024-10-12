using PokeAPI.Pokemon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelUpWindow : MonoBehaviour
{

    public GameObject levelUpPanel;
    public GameObject levelUpCanvas;
    public PokeDex pokemonPool;
    public List<GameObject> MovePool;
    public List<string> addedTypes;

    int maxMoves = 6;

    List<int> Rarity = new List<int>();
    List<int> typeTracker = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        //foreach(GameObject g in MovePool)
        //{
        //    g.GetComponent<Move>().moveType = g.GetComponent<Move>().moveType.ToLower();
        //}
    }

    private void OnEnable()
    {
        //for(int j = 0; j < 100; j++)
        //{
        Rarity.Add(500);
        Rarity.Add(550);
        Rarity.Add(700);
        Time.timeScale = 0f;
        List<int> choices = new List<int>();
        List<GameObject> chosenMoves = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            int trys = 0;
            while (choices.Count <= i)
            {
                int rInt = UnityEngine.Random.Range(0, MovePool.Count);
                if (!choices.Contains(rInt))
                {
                    if (addedTypes.Count >= maxMoves)
                    {
                        if (addedTypes.Contains(MovePool[rInt].GetComponent<Move>().moveType))
                        {
                            choices.Add(rInt);
                            chosenMoves.Add(MovePool[rInt]);
                        }
                    }
                    else
                    {
                        choices.Add(rInt);
                        chosenMoves.Add(MovePool[rInt]);
                    }
                }
                trys++;
                if (trys > 1000)
                {
                    return;
                }
            }
        }
        if (typeTracker.Count == 0)
        {
            for (int i = 0; i < MovePool.Count; i++)
            {
                typeTracker.Add(0);
            }
        }
        for (int i = 0; i < choices.Count; i++)
        {
            typeTracker[choices[i]]++;
        }
        foreach (int i in typeTracker)
        {
            //Debug.Log(i);
        }
        //foreach (int i in choices)
        //{
        //    Debug.Log(i);
        //}
        //foreach (GameObject i in chosenMoves)
        //{
        //    Debug.Log(i.name);
        //}
        List<int> chosenPokemon = new List<int>();
        for (int i = 0; i < chosenMoves.Count; i++)
        {
            List<PokeDex.PokeDexEntry> possiblePokemon = new List<PokeDex.PokeDexEntry>();
            int trys = 0;
            while (possiblePokemon.Count == 0)
            {
                int baseStatTotalRoll = UnityEngine.Random.Range(150, 575);
                int baseStatFloor = 0;
                int baseStatCap = Rarity[0];
                if (baseStatTotalRoll > Rarity[0])
                {
                    baseStatFloor = Rarity[0];
                    baseStatCap = Rarity[1];
                }
                if (baseStatTotalRoll > Rarity[1])
                {
                    baseStatFloor = Rarity[1];
                    baseStatCap = Rarity[2];
                }
                //Debug.Log(baseStatCap);
                string type = chosenMoves[i].GetComponent<Move>().moveType;
                possiblePokemon = GetValidPokemon(type, baseStatCap, baseStatFloor);
                trys++;
                if (trys > 100)
                {
                    return;
                }
            }
            int rInt = UnityEngine.Random.Range(0, possiblePokemon.Count);
            chosenPokemon.Add((int)possiblePokemon[rInt].id);
        }


        for (int i = 0; i < chosenMoves.Count; i++)
        {
            GameObject m = chosenMoves[i];
            GameObject pokemon = GameObject.Find(m.name + ("(Clone)"));

            GameObject panel = Instantiate(levelUpPanel);
            panel.transform.SetParent(levelUpCanvas.transform, false);
            LevelUpPanel l = panel.GetComponent<LevelUpPanel>();
            if (pokemon == null)
            {

                l.newPokemon = true;
                Move moveScript = m.GetComponent<Move>();
                moveScript.pokemon = pokemonPool.pokeDex[chosenPokemon[i] - 1];
                l.setPokemon(moveScript, GetRarityColor(GetBaseStatTotal(GetHighestEvo(moveScript.pokemon))));
                l.levelUpWindow = gameObject;

                l.SetImage(chosenPokemon[i]);
            }
            else
            {
                l.newPokemon = false;
                l.setPokemon(pokemon.GetComponent<Move>(), GetRarityColor(GetBaseStatTotal(GetHighestEvo(pokemon.GetComponent<Move>().pokemon))));
                l.levelUpWindow = gameObject;

                l.SetImage((int)pokemon.GetComponent<Move>().pokemon.id);
            }
        }
        //foreach (GameObject p in GameObject.FindGameObjectsWithTag("Pokemon"))
        //{
        //    GameObject panel = Instantiate(levelUpPanel);
        //    panel.transform.SetParent(levelUpCanvas.transform, false);
        //    LevelUpPanel l = panel.GetComponent<LevelUpPanel>();
        //    l.setPokemon(p.GetComponent<Pokemon>());
        //    l.levelUpWindow = gameObject;
        //}
        //}

    }

    int GetBaseStatTotal(PokeDex.PokeDexEntry p)
    {
        return (int)(p.@base.HP + p.@base.Attack + p.@base.SpAttack + p.@base.Defense + p.@base.SpDefense + p.@base.Speed) ;
    }
    PokeDex.PokeDexEntry GetHighestEvo(PokeDex.PokeDexEntry p)
    {
        PokeDex.PokeDexEntry highestEvo = p;
        if(p.evolution.next != null)
        {
            long evoID;
            Int64.TryParse(p.evolution.next[0][0], out evoID);
            if(evoID <= pokemonPool.pokeDex.Count)
            {
                if (pokemonPool.pokeDex[(int)evoID - 1].@base != null)
                {
                    highestEvo = GetHighestEvo(pokemonPool.pokeDex[(int)evoID - 1]);
                }
            }
        }
        return highestEvo;
    }
    int whiteTotal = 0;
    int cyanTotal = 0;
    int magentaTotal = 0;
    Color GetRarityColor(int baseStatTotal)
    {
        Color c = Color.white;
        if(baseStatTotal >= Rarity[0])
        {
            c = Color.cyan;
        }
        if(baseStatTotal >= Rarity[1])
        {
            c = Color.magenta;
        }
        //if (c == Color.magenta)
        //{
        //    magentaTotal++;
        //}
        //if (c == Color.cyan)
        //{
        //    cyanTotal++;
        //}
        //if (c == Color.white)
        //{
        //    whiteTotal++;
        //}
        //Debug.Log(whiteTotal + " " + cyanTotal + " " + magentaTotal);
        return c;
    }
    List<PokeDex.PokeDexEntry> GetValidPokemon(string type, int baseStatCap, int baseStatFloor)
    {
        List<PokeDex.PokeDexEntry> validPokemon = new List<PokeDex.PokeDexEntry>();
        for( int i = 0; i < pokemonPool.pokeDex.Count; i ++) //Max 809
        {
            PokeDex.PokeDexEntry p = pokemonPool.pokeDex[i];
            if (//true)
                p.evolution.prev == null
                && p.type.Contains(type)
                && GetBaseStatTotal(GetHighestEvo(p)) <= baseStatCap
                && GetBaseStatTotal(GetHighestEvo(p)) >= baseStatFloor)
            {
                validPokemon.Add(p);
            }
            //if (
            //    p.type.Contains(type))
            //{
            //    Debug.Log(p.name.english);
            //    Debug.Log(GetBaseStatTotal(GetHighestEvo(p)));
            //}
        }
        if (validPokemon.Count == 0)
        {
            //Debug.LogError(type);
            //Debug.LogError(baseStatCap);
            //Debug.LogError(baseStatFloor);
            Debug.LogError("could not find any" + type + baseStatCap.ToString());
        }
        return validPokemon;
    }

    private void OnDisable()
    {
        foreach(LevelUpPanel l in levelUpCanvas.GetComponentsInChildren<LevelUpPanel>())
        {
            Destroy(l.gameObject);
        }
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

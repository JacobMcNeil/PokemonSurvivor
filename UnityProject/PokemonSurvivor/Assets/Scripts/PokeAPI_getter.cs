using Newtonsoft.Json;
using PokeAPI;
using PokeAPI.EvoChain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static PokeDex;

public class PokeAPI_getter : MonoBehaviour
{
    PokeDex.PokeDexEntry p;
    // Start is called before the first frame update
    void Start()
    {
        string fileName = "Assets\\MyPokeDex.json";
        string PokeAPI_PokemonJson = "Assets\\PokeAPI_Pokemon.json";
        string PokeAPI_SpeciesJson = "Assets\\PokeAPI_Species.json";
        string PokeAPI_EvoChainJson = "Assets\\PokeAPI_EvoChain.json";
        string PokeAPI_SpritesPath = "Assets\\Sprites\\";
        //GetAllPokemon(PokeAPI_PokemonJson);
        //GetAllSpecies(PokeAPI_SpeciesJson);
        //GetAllEvoChain(PokeAPI_EvoChainJson);
        //GetSprites(PokeAPI_SpritesPath);

        string json = File.ReadAllText(PokeAPI_PokemonJson);
        List<PokeAPI.Pokemon.Pokemon> test = JsonConvert.DeserializeObject<List<PokeAPI.Pokemon.Pokemon>>(json, PokeAPI.Pokemon.Converter.Settings);

        string SpeciesJson = File.ReadAllText(PokeAPI_SpeciesJson);
        List<PokeAPI.Species.Species> species = JsonConvert.DeserializeObject<List<PokeAPI.Species.Species>>(SpeciesJson, PokeAPI.Pokemon.Converter.Settings);

        string EvoChainJson = File.ReadAllText(PokeAPI_EvoChainJson);
        List<PokeAPI.EvoChain.EvoChain> EvoChain = JsonConvert.DeserializeObject<List<PokeAPI.EvoChain.EvoChain>>(EvoChainJson, PokeAPI.EvoChain.Converter.Settings);

        File.WriteAllText(fileName, "[");
        for (int i = 0; i < 1025; i++) //not 899 - 905//549 max evochain 1025 max pokemon id
        {
            if (i != 0)
            {
                File.AppendAllText(fileName, ",");
            }
            p = new PokeDex.PokeDexEntry();
            p.id = test[i].Id;
            p.name = new PokeDex.Name();
            p.name.english = test[i].Name;
            p.name.japanese = test[i].Name;
            p.name.chinese = test[i].Name;
            p.name.french = test[i].Name;
            p.type = new List<string>();
            foreach (PokeAPI.Pokemon.TypeElement t in test[i].Types)
            {
                p.type.Add(t.Type.Name);
            }
            p.@base = new Base();
            p.@base.HP = test[i].Stats[0].BaseStat;
            p.@base.Attack = test[i].Stats[1].BaseStat;
            p.@base.Defense = test[i].Stats[2].BaseStat;
            p.@base.SpAttack = test[i].Stats[3].BaseStat;
            p.@base.SpDefense = test[i].Stats[4].BaseStat;
            p.@base.Speed = test[i].Stats[5].BaseStat;
            Debug.Log(species[i].EvolutionChain.Url.Segments.Last().TrimEnd('/'));
            Debug.Log(EvoChain[0].Id);
            EvoChain evoChain = FindEvoChain(species[i].EvolutionChain.Url.Segments.Last().TrimEnd('/'), EvoChain);
            getEvo(evoChain.Chain, species[i]);
            if (p.evolution.prev == null || p.evolution.prev.Count == 0 )
            {
                Debug.Log("here");
                p.evolution.stage = 1;
            }
            else
            if (p.evolution.prev.Count == 1)
            {
                p.evolution.stage = 2;
            }
            else
            if (p.evolution.prev.Count == 2)
            {
                p.evolution.stage = 3;
            }
            else
            if (p.evolution.prev.Count == 3)
            {
                p.evolution.stage = 4;
            }
            if (evoChain.Chain.IsBaby)
            {
                Debug.Log("baby");
                p.evolution.stage -= 1;
            }
            Debug.Log(JsonConvert.SerializeObject(p, PokeDex.Converter.Settings));

            //foreach (PokeAPI.Pokemon.Pokemon p in test)
            //{
            //    Debug.Log(p.Name);
            //}
            File.AppendAllText(fileName, JsonConvert.SerializeObject(p, PokeDex.Converter.Settings));

        }
        File.AppendAllText(fileName, "]");
    }

    void GetSprites(string path)
    {
        for(int i = 1; i <= 1025; i++)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile($"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + i + ".png", path + i + ".png");
        }
    }
    EvoChain FindEvoChain(string id, List<EvoChain> ec)
    {
        foreach(EvoChain eci in ec)
        {
            if (eci.Id.ToString() == id)
            {
                return eci;
            }
        }
        return null;
    }
    public void GetAllPokemon(string PokeAPI_PokemonJson)
    {
        File.WriteAllText(PokeAPI_PokemonJson, "[");
        for (int i = 1; i <= 1025; i++)
        {
            if (i != 1)
            {
                File.AppendAllText(PokeAPI_PokemonJson, ",");
            }
            string json = new WebClient().DownloadString($"https://pokeapi.co/api/v2/pokemon/" + i.ToString());
            File.AppendAllText(PokeAPI_PokemonJson, json);
        }
        File.AppendAllText(PokeAPI_PokemonJson, "]");
    }
    public void GetAllSpecies(string PokeAPI_SpeciesJson)
    {
        File.WriteAllText(PokeAPI_SpeciesJson, "[");
        for (int i = 1; i <= 1025; i++)
        {
            if (i != 1)
            {
                File.AppendAllText(PokeAPI_SpeciesJson, ",");
            }
            string json = new WebClient().DownloadString($"https://pokeapi.co/api/v2/pokemon-species/" + i.ToString());
            File.AppendAllText(PokeAPI_SpeciesJson, json);
        }
        File.AppendAllText(PokeAPI_SpeciesJson, "]");
    }
    public void GetAllEvoChain(string PokeAPI_EvoChainJson)
    {
        File.WriteAllText(PokeAPI_EvoChainJson, "[");
        for (int i = 1; i <= 549; i++)
        {
            try
            {
                string json = new WebClient().DownloadString($"https://pokeapi.co/api/v2/evolution-chain/" + i.ToString());
                if (i != 1)
                {
                    File.AppendAllText(PokeAPI_EvoChainJson, ",");
                }
                File.AppendAllText(PokeAPI_EvoChainJson, json);
            }
            catch 
            {
                Debug.Log(i);
            }
        }
        File.AppendAllText(PokeAPI_EvoChainJson, "]");
    }
    void getEvo(Chain c, PokeAPI.Species.Species s)
    {
        if(p.evolution == null)
        {
            p.evolution = new Evolution();
        }
        if (c.Species.Name == s.Name)
        {
            if (c.EvolvesTo.Count() > 0)
            {
                p.evolution.next = new List<List<string>>();
                foreach (Chain c2 in c.EvolvesTo)
                {
                    List<String> entry = new List<string>();
                    entry.Add(c2.Species.Url.Segments.Last().TrimEnd('/'));
                    if(c2.EvolutionDetails.Length > 0)
                    {
                        if (c2.EvolutionDetails[0].MinLevel != null)
                        {
                            entry.Add(c2.EvolutionDetails[0].MinLevel.ToString());
                        }
                        else
                        if (c2.EvolutionDetails[0].Item != null)
                        {
                            entry.Add(c2.EvolutionDetails[0].Item.Name.ToString());
                        }
                        else
                        if (c2.EvolutionDetails[0].Trigger.Name == "trade")
                        {
                            entry.Add("trade");
                        }
                        else
                        if (c2.EvolutionDetails[0].MinHappiness != null)
                        {
                            entry.Add("friendship");
                        }
                        else
                        {
                            entry.Add("other");
                        }
                    }
                    else
                    {
                        entry.Add("other");
                    }
                    p.evolution.next.Add(entry);
                }
            }
        }
        else
        {
            foreach (Chain c3 in c.EvolvesTo)
            {
                if (p.evolution.prev == null)
                {
                    p.evolution.prev = new List<string>();
                }
                p.evolution.prev.Add(c.Species.Url.Segments.Last().TrimEnd('/'));
                p.evolution.prev = p.evolution.prev.Distinct().ToList();
                getEvo(c3, s);
            }
        }
    }
     
    int GetId(string name)
    {
        if (name == "wormadam")
        {
            return 413;
        }
        if (name == "dudunsparce")
        {
            return 982;
        }
        if (name == "basculegion")
        {
            return 902;
        }
        Debug.Log($"https://pokeapi.co/api/v2/pokemon/" + name);
        string json = new WebClient().DownloadString($"https://pokeapi.co/api/v2/pokemon/" + name);
        PokeAPI.Pokemon.Pokemon test = JsonConvert.DeserializeObject<PokeAPI.Pokemon.Pokemon>(json, PokeAPI.Pokemon.Converter.Settings);
        return (int)test.Id;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

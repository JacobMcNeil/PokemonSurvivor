using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface Move
{
    //  damage, cooldown, area
    //  projectile speed,duration, amount
    // knockback, tick rate, pierce
    //ember: damage, cooldown, area, duration, amount, tick rate
    //bubble: damage, cooldown, area, amount, speed
    //spark: damage, area, tick rate
    //razor: damage, area, speed, cooldown, pierce 
    public string moveName { get; set; }
    public string moveType { get; set; }
    public List<moveStat> moveStats {  get; set; }
    public PartyController partyController { get; set; }

    public int totalDamage {  get; set; }
    public float timeActive {  get; set; }

    public class moveStat
    {
        public string statName;
        public int statPoints;
        public Func<int,float> Calc;
    }

    public PokeDex.PokeDexEntry pokemon {  get; set; }

    public PokeDex pokeDex { get; set; }

    public Player player { get; set; }

    public void Evolve()
    {
        if(pokemon.evolution.next == null || player.everStoneAmount >= 1)
        {
            return;
        }
        if (pokemon.evolution.next.Count > 0)
        {
            Debug.Log(pokemon.evolution.next[0][0]);
            long nextlevel;

            Int64.TryParse(pokemon.evolution.next[0][1], out nextlevel);
            long evoID;
            Int64.TryParse(pokemon.evolution.next[0][0], out evoID);
            if(nextlevel == 0)
            {
                if (pokemon.evolution.next[0][1].Contains("fire-stone"))
                {
                    if (player.fireStoneAmount > 0)
                    {
                        nextlevel = 0;
                    }
                }
                else
                if (pokemon.evolution.next[0][1].Contains("water-stone"))
                {
                    if (player.fireStoneAmount > 0)
                    {
                        if (player.waterStoneAmount > 0)
                        {
                            nextlevel = 0;
                        }
                    }
                }
                else
                if (pokemon.evolution.next[0][1].Contains("thunder-stone"))
                {
                    if (player.thunderStoneAmount > 0)
                    {
                        nextlevel = 0;
                    }
                }
                else
                if (pokemon.evolution.next[0][1].Contains("leaf-stone"))
                {
                    if (player.leafStoneAmount > 0)
                    {
                        nextlevel = 0;
                    }
                }
                else
                if (pokemon.evolution.next[0][1].Contains("trade") && !pokemon.evolution.next[0][1].Contains("Trade h"))
                {
                    if (player.tradeCableAmount > 0)
                    {
                        nextlevel = 0;
                    }
                }
                else
                if (pokemon.evolution.next[0][1].Contains("friendship"))
                {
                    if (player.sootheBellAmount > 0)
                    {
                        nextlevel = 0;
                    }
                }
                else
                {
                    nextlevel = 35;
                }
            }
            int level = GetLevel();
            if (level >= nextlevel)
            {
                Debug.Log("Evo");
                pokemon = pokeDex.pokeDex[(int)(evoID - 1)];
                partyController.PartyUpdated.Invoke();
            }
        }
    }
    public int GetLevel()
    {
        int level = 0;
        foreach (moveStat m in moveStats)
        {
            level += m.statPoints * 5;
        }
        return level;
    }
}

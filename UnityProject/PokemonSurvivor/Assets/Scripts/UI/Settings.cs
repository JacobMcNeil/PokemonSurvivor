using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static public class Settings
{
    public static bool freePlay;
    public static int stageIndex;
    public static List<int> unlockedPokemon;
    public static int highestStageBeat;

    public static void AddPokemon(int id)
    {
        if (!freePlay)
        {
            unlockedPokemon.Add(id);
            unlockedPokemon = unlockedPokemon.Distinct().ToList();
        }
    }
    public static void saveHighestStageBeat()
    {
        string test = JsonConvert.SerializeObject(Settings.highestStageBeat);
        //Debug.LogError(test);
        //Debug.LogError(Application.persistentDataPath);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/highstage", test);
    }

    public static int loadHighestStageBeat()
    {
        if (!System.IO.File.Exists(Application.persistentDataPath + "/highstage"))
        {
            System.IO.File.WriteAllText(Application.persistentDataPath + "/highstage", "-1");
        }
        string file = System.IO.File.ReadAllText(Application.persistentDataPath + "/highstage");
        int i = JsonConvert.DeserializeObject<int>(file);
        return i;
    }
}

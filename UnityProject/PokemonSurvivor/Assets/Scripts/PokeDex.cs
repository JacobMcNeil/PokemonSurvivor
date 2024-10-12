using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public class PokeDex : MonoBehaviour
{
    public List<PokeDexEntry> pokeDex;
    [SerializeField]
    public PokeDexEntry pokeDexEntry;
    public TextAsset pokeDexJson;
    // Start is called before the first frame update
    void Awake()
    {
        //string path = "Assets\\pokedex.json";
        //string json = File.ReadAllText(path);
        //Debug.Log(json);
        //pokeDexList _dataClass = JsonUtility.FromJson<pokeDexList>("{\"t\":" + json + "}");
        //foreach (Welcome p in _dataClass.t)
        //{
        //    Debug.Log(p.Id);
        //}
        //Welcome _dataClass = JsonUtility.FromJson<Welcome>(json);
        //Debug.Log(_dataClass.Type.Count);


        //pokeDex = JsonUtility.FromJson<pokeDexList>("{\"entries\":" + json + "}");
        pokeDex = JsonConvert.DeserializeObject<List<PokeDexEntry>>(pokeDexJson.text, Converter.Settings);
        pokeDexEntry = pokeDex[0];
        //foreach (PokeDexEntry p in pokeDex)
        //{
        //    if (p.evolution.next != null)
        //    {

        //        Debug.Log(p.evolution.next[0][0]);
        //        Debug.Log(p.evolution.next[0][1]);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //[Serializable]
    public class pokeDexList
    {
        public List<PokeDexEntry> entries;
    }
    //[Serializable]
    //public partial class PokeDexEntry
    //{
    //    public int id;
    //    public Name name;
    //    public List<string> type;
    //    public BaseStats baseStats;
    //}

    //[Serializable]
    //public partial class BaseStats
    //{
    //    public long Hp;
    //    public long Attack;
    //    public long Defense;
    //    public long SpAttack;
    //    public long SpDefense;
    //    public long Speed;
    //}

    //[Serializable]
    //public partial class Name
    //{
    //    public string english;
    //    public string japanese;
    //    public string chinese;
    //    public string french;   
    //}
    //[Serializable]
    //public partial class PokeDexEntry
    //{
    //    public long id;
    //    public Name name;
    //    public List<string> type;
    //    public Base @base;
    //    public string species;
    //    public string description;
    //    public Evolution evolution;
    //    public Profile profile;
    //}

    //[Serializable]
    //public partial class Base
    //{
    //    public long HP;
    //    public long Attack;
    //    public long Defense;
    //    public long SpAttack;
    //    public long SpDefense;
    //    public long Speed;
    //}

    //[Serializable]
    //public partial class Evolution
    //{
    //    public List<int> prev;
    //    public List<NextEvolution> next;
    //}
    //[Serializable]
    //public partial class NextEvolution
    //{
    //    public List<int> nextEvolution;
    //}

    //[Serializable]
    //public partial class Name
    //{
    //    public string english;
    //    public string japanese;
    //    public string chinese;
    //    public string french;
    //}

    //[Serializable]
    //public partial class Profile
    //{
    //    public string height;
    //    public string weight;
    //    public List<string> egg;
    //    public List<List<string>> ability;
    //    public string gender;
    //}
    [Serializable]
    public partial class PokeDexEntry
    {
        [JsonProperty("id")]
        [field: SerializeField]
        public long id { get; set; }

        [JsonProperty("name")]
        [field: SerializeField]
        public Name name { get; set; }

        [JsonProperty("type")]
        [field: SerializeField]
        public List<string> type { get; set; }

        [JsonProperty("base")]
        [field: SerializeField]
        public Base @base { get; set; }

        [JsonProperty("species")]
        [field: SerializeField]
        public string species { get; set; }

        [JsonProperty("description")]
        [field: SerializeField]
        public string description { get; set; }

        [JsonProperty("evolution")]
        [field: SerializeField]
        public Evolution evolution { get; set; }

        [JsonProperty("profile")]
        [field: SerializeField]
        public Profile profile { get; set; }
    }

    [Serializable]
    public partial class Base
    {
        [JsonProperty("HP")]
        [field: SerializeField]
        public long HP { get; set; }

        [JsonProperty("Attack")]
        [field: SerializeField]
        public long Attack { get; set; }

        [JsonProperty("Defense")]
        [field: SerializeField]
        public long Defense { get; set; }

        [JsonProperty("Sp. Attack")]
        [field: SerializeField]
        public long SpAttack { get; set; }

        [JsonProperty("Sp. Defense")]
        [field: SerializeField]
        public long SpDefense { get; set; }

        [JsonProperty("Speed")]
        [field: SerializeField]
        public long Speed { get; set; }
    }

    [Serializable]
    public partial class Evolution
    {
        [JsonProperty("prev")]
        [field: SerializeField]
        public List<string> prev { get; set; }

        [JsonProperty("next")]
        [field: SerializeField]
        public List<List<string>> next { get; set; }
        [JsonProperty("stage")]
        [field: SerializeField]
        public long stage { get; set; }
    }

    [Serializable]
    public partial class Name
    {
        [JsonProperty("english")]
        [field: SerializeField]
        public string english { get; set; }

        [JsonProperty("japanese")]
        [field: SerializeField]
        public string japanese { get; set; }

        [JsonProperty("chinese")]
        [field: SerializeField]
        public string chinese { get; set; }

        [JsonProperty("french")]
        [field: SerializeField]
        public string french { get; set; }
    }

    [Serializable]
    public partial class Profile
    {
        [JsonProperty("height")]
        [field: SerializeField]
        public string height { get; set; }

        [JsonProperty("weight")]
        [field: SerializeField]
        public string weight { get; set; }

        [JsonProperty("egg")]
        [field: SerializeField]
        public List<string> egg { get; set; }

        [JsonProperty("ability")]
        [field: SerializeField]
        public List<List<string>> ability { get; set; }

        [JsonProperty("gender")]
        [field: SerializeField]
        public string gender { get; set; }
    }

    //public partial class Welcome
    //{
    //    public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, Converter.Settings);
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, Converter.Settings);
    //}

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button start_B;
    [SerializeField]
    Toggle freePlay_T;
    [SerializeField]
    TMPro.TMP_Dropdown stage_DD;

    public GameObject pokemonGrid;

    // Start is called before the first frame update
    void Start()
    {
        start_B.onClick.AddListener(StartRun);
        freePlay_T.onValueChanged.AddListener(UpdateFreeplaySetting);
        stage_DD.onValueChanged.AddListener(UpdateStageIndex);

        Settings.freePlay = freePlay_T.isOn;
        Settings.stageIndex = stage_DD.value;
        Settings.highestStageBeat = Settings.loadHighestStageBeat();
        while (stage_DD.options.Count > Settings.highestStageBeat + 2)
        {
            stage_DD.options.RemoveAt(stage_DD.options.Count - 1);
        }

        Settings.unlockedPokemon = loadUnlockedPokemon();
        foreach (int i in Settings.unlockedPokemon)
        {
            GameObject go = new GameObject();
            Image image =  go.AddComponent<Image>();
            go.AddComponent<LayoutElement>();
            image.sprite = GetSprite(i);

            go.transform.SetParent(pokemonGrid.transform, false);
            go.transform.localScale = Vector3.one * 2;
        }
    }
    void StartRun()
    {
        SceneManager.LoadScene("SampleScene");
    }
    void UpdateFreeplaySetting(bool value)
    {
        Settings.freePlay = value;
    }
    void UpdateStageIndex(int value)
    {
        Settings.stageIndex = value;
    }
    public Sprite GetSprite(int pokemonId)
    {
        string spriteFileString = pokemonId.ToString();
        Sprite sprite = Resources.Load<Sprite>("sprites/" + spriteFileString);
        return sprite;
    }
    public List<int> loadUnlockedPokemon()
    {
        if (!System.IO.File.Exists(Application.persistentDataPath + "/saveData"))
        {
            System.IO.File.WriteAllText(Application.persistentDataPath + "/saveData", "[1,4,7]");
        }
        string file = System.IO.File.ReadAllText(Application.persistentDataPath + "/saveData");
        List<int> ints = JsonConvert.DeserializeObject<List<int>>(file);
        return ints;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI pokemonName_L;
    //[SerializeField]
    //TextMeshProUGUI baseDmg_L;
    //[SerializeField]
    //TextMeshProUGUI coolDown_L;
    //[SerializeField]
    //Button baseDmg_B;
    //[SerializeField]
    //Button coolDown_B;
    [SerializeField]
    Button addToParty_B;
    bool _newPokemon;

    public PartyController partyController; 

    public bool newPokemon { get { return _newPokemon; } set { _newPokemon = value; NewPokemon(); } }

    public GameObject newPokemonSection;
    public GameObject oldPokemonSection;
    public GameObject statLine;
    public Image image;
    public Image backgroundImage;
    public Image caught;

    private void NewPokemon()
    {
        if (newPokemon)
        {
            newPokemonSection.SetActive(true);
            oldPokemonSection.SetActive(false);
        }
        else
        {
            newPokemonSection.SetActive(false);
            oldPokemonSection.SetActive(true);
        }
    }

    public GameObject levelUpWindow;

    public Move move;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        addToParty_B.onClick.AddListener(AddPokemonToParty);
        partyController = GameObject.Find("Party").GetComponent<PartyController>();
        if (Settings.unlockedPokemon.Contains((int)move.pokemon.id))
        {
            caught.enabled = true;
        }
        else
        {
            caught.enabled = false;
        }
    }

    private void AddPokemonToParty()
    {
        //Debug.Log(levelUpWindow.GetComponent<LevelUpWindow>());
        //Debug.Log(levelUpWindow.GetComponent<LevelUpWindow>().MovePool.Find(p => p.name == move.moveName));
        GameObject AddedPokemon = Instantiate(levelUpWindow.GetComponent<LevelUpWindow>().MovePool.Find(p => p.name == move.moveName));
        if(move.moveName == "Revenge")
        {
            player.revenge = AddedPokemon.GetComponent<Revenge>();
        }
        if (move.moveName == "Iron Defence")
        {
            player.ironDefence = AddedPokemon.GetComponent<IronDefence>();
        }
        levelUpWindow.GetComponent<LevelUpWindow>().addedTypes.Add(move.moveType);
        Settings.AddPokemon((int)move.pokemon.id);
        AddedPokemon.GetComponent<Move>().pokemon = move.pokemon;
        AddedPokemon.transform.SetParent(player.transform, false);
        levelUpWindow.SetActive(false);
        partyController.PartyUpdated.Invoke();
    }

    private void UpgradedCoolDown()
    {
        //move.coolDown = move.coolDown * .9f;
        levelUpWindow.SetActive(false);
    }

    private void UpgradeBaseDmg()
    {
        //move.baseDmg += 1;
        levelUpWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPokemon(Move p, Color c)
    {
        move = p;
        int moveLevel = 0;
        foreach(Move.moveStat m in p.moveStats)
        {
            moveLevel += m.statPoints * 5;
        }
        pokemonName_L.text = p.moveName + System.Environment.NewLine + p.pokemon.name.english + " Lv. " + moveLevel + " Dmg: " + p.totalDamage;
        foreach(Move.moveStat m in p.moveStats)
        {
            GameObject s = Instantiate(statLine);
            s.transform.SetParent(oldPokemonSection.transform, false);
            s.GetComponent<StatLine>().moveStat = m;
            s.GetComponent<StatLine>().levelUpPanel = this;
        }
        backgroundImage.color = c;
        //baseDmg_L.text = p.baseDmg.ToString();
        //coolDown_L.text = p.coolDown.ToString();
    }

    public void SetImage(int pokemonId)
    {
        image.sprite = GetSprite(pokemonId);
    }

    public Sprite GetSprite(int pokemonId)
    {
        string spriteFileString = pokemonId.ToString();

        //while (spriteFileString.Count() < 3)
        //{
        //    spriteFileString = "0" + spriteFileString;
        //}
        Sprite sprite = Resources.Load<Sprite>("sprites/" + spriteFileString);
        return sprite;
    }
}

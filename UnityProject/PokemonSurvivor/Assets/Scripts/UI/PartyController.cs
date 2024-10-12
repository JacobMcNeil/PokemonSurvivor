using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PartyController : MonoBehaviour
{
    public List<GameObject> partySlots = new List<GameObject>();
    public List<TMP_Text> partyLevel = new List<TMP_Text>();

    public UnityEvent PartyUpdated;
    // Start is called before the first frame update
    void Start()
    {
        partySlots = GameObject.FindGameObjectsWithTag("PartySlot").ToList();
        partySlots = partySlots.OrderBy(s => s.transform.parent.GetSiblingIndex()).ToList();
        PartyUpdated.AddListener(UpdateParty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateParty()
    {
        List<GameObject> currentMoves = new List<GameObject>();
        currentMoves = GameObject.FindGameObjectsWithTag("Pokemon").ToList();
        foreach (GameObject slot in partySlots) 
        {
            slot.GetComponent<SpriteRenderer>().sprite = null;
        }
        for (int i = 0; i < currentMoves.Count; i++)
        {
            Move m = currentMoves[i].GetComponent<Move>();
            partySlots[i].GetComponent<SpriteRenderer>().sprite = GetSprite((int)m.pokemon.id);
            partyLevel[i].text = "Level " + currentMoves[i].GetComponent<Move>().GetLevel();
        }
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

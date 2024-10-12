using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Tilemaps;
using UnityEngine;

public class TrainerCardWindow : MonoBehaviour
{
    public GameObject trainerCardPanel;
    public GameObject tranerCardCanvas;
    [SerializeField]
    public List<TrainerCard> cardPool = new List<TrainerCard>();
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        TrainerCard card = new TrainerCard();
        card.cardName = "Potion";
        card.amount = 20;
        card.cardDescription = "Heal " + card.amount + ".";
        int tempAmount = (int)card.amount;
        card.action = () => { player.SetHp((int)(player.health + 20)); };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Supper Potion";
        card.amount = 60;
        card.cardDescription = "Heal " + card.amount + ".";
        card.action = () => { player.SetHp((int)(player.health + 60)); };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Hyper Potion";
        card.amount = 120;
        card.cardDescription = "Heal " + card.amount + ".";
        card.action = () => { player.SetHp((int)(player.health + 120)); };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "HP Up";
        card.amount = 10;
        card.cardDescription = "Increase max health by " + card.amount + "%.";
        card.action = () => { player.maxHealth = (int)(player.maxHealth * 1.1); player.SetHp((int)player.health); };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Protein";
        card.amount = 3;
        card.cardDescription = "Gain " + card.amount + " might.";
        card.action = () => { player.might += 3; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Leftover";
        card.amount = 1;
        card.cardDescription = "Gain " + card.amount + " health regen.";
        card.action = () => { player.healthRegen += 1; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Leftover +";
        card.amount = 2;
        card.cardDescription = "Gain " + card.amount + " health regen.";
        card.action = () => { player.healthRegen += 2; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Firestone";
        card.amount = 5;
        card.cardDescription = "Gain " + card.amount + "% fire damage increase.";
        card.action = () => { player.fireStoneAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Thunderstone";
        card.amount = 5;
        card.cardDescription = "Gain " + card.amount + "% electric damage increase.";
        card.action = () => { player.thunderStoneAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Leafstone";
        card.amount = 5;
        card.cardDescription = "Gain " + card.amount + "% grass damage increase.";
        card.action = () => { player.leafStoneAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Waterstone";
        card.amount = 5;
        card.cardDescription = "Gain " + card.amount + "% water damage increase.";
        card.action = () => { player.waterStoneAmount++; };
        cardPool.Add(card);
        card = new TrainerCard();

        card.cardName = "Dragon Fang";
        card.amount = 5;
        card.cardDescription = "Gain " + card.amount + "% dragon damage increase.";
        card.action = () => { player.dragonFangAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Toxic Barb";
        card.amount = 5;
        card.cardDescription = "Gain " + card.amount + "% poison damage increase.";
        card.action = () => { player.toxicBarbAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Hard Stone";
        card.amount = 5;
        card.cardDescription = "Gain " + card.amount + "% rock damage increase.";
        card.action = () => { player.hardStoneAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Never Melt Ice";
        card.amount = 5;
        card.cardDescription = "Gain " + card.amount + "% frozen duration increase.";
        card.action = () => { player.neverMeltAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Trade Cable";
        card.amount = 5;
        card.cardDescription = "Causes some pokemon to evolve.";
        card.action = () => { player.tradeCableAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Soothe Bell";
        card.amount = 5;
        card.cardDescription = "Causes some pokemon to evolve.";
        card.action = () => { player.sootheBellAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Everstone";
        card.amount = 5;
        card.cardDescription = "Pokemon can no longer evolve but all not fully evolved pokemon gain 5% boost to all stats.";
        card.action = () => { player.everStoneAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Common Boost";
        card.amount = 5;
        card.cardDescription = "All common pokemon get a 5% boost to all stats.";
        card.action = () => { player.commonBoostAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Rare Boost";
        card.amount = 5;
        card.cardDescription = "All rare pokemon get a 5% boost to all stats.";
        card.action = () => { player.rareBoostAmount++; };
        cardPool.Add(card);

        card = new TrainerCard();
        card.cardName = "Item Finder";
        card.amount = 5;
        card.cardDescription = "Increase item collection radius by 50%";
        card.action = () => { player.itemFinderAmount++; };
        cardPool.Add(card);

        gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        Debug.Log("TrainerCardWindow");
        List<int> choices = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int trys = 0;
            while (choices.Count <= i)
            {
                int rInt = UnityEngine.Random.Range(0, cardPool.Count);
                if (!choices.Contains(rInt))
                {
                    choices.Add(rInt);
                }
                trys++;
                if (trys > 10000)
                {
                    return;
                }
            }
        }
        for (int i= 0; i < choices.Count; i++)
        {
            GameObject panel = Instantiate(trainerCardPanel);
            panel.transform.SetParent(tranerCardCanvas.transform, false);
            panel.GetComponent<TrainerCardPanel>().trainerCardWindow = gameObject;
            panel.GetComponent<TrainerCardPanel>().SetTrainerCard(cardPool[choices[i]]);
        }
        Time.timeScale = 0;
    }
    private void OnDisable()
    {
        foreach (TrainerCardPanel l in tranerCardCanvas.GetComponentsInChildren<TrainerCardPanel>())
        {
            Destroy(l.gameObject);
        }
        foreach (Transform l in tranerCardCanvas.GetComponentsInChildren<Transform>())
        {
            if (l.gameObject.name != "Canvas")
            {
                Destroy(l.gameObject);
            }
        }
        Time.timeScale = 1f;
    }
}

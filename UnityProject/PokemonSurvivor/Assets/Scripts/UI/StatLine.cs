using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLine : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI statName_L;
    [SerializeField]
    TextMeshProUGUI statValue_L;
    [SerializeField]
    Button levelUp_B;

    Move.moveStat _moveStat;
    public LevelUpPanel levelUpPanel;
    PartyController partyController;
    public Move.moveStat moveStat
    {
        get
        {
            return _moveStat;
        }
        set
        {
            _moveStat = value;
            statName_L.text = _moveStat.statName + ": " + moveStat.statPoints;
            statValue_L.text = MathF.Round(_moveStat.Calc(moveStat.statPoints), 2).ToString() + "=>"+ MathF.Round(_moveStat.Calc(moveStat.statPoints + 1), 3).ToString();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        levelUp_B.onClick.AddListener(StatSelected);
        partyController = GameObject.Find("Party").GetComponent<PartyController>();
    }

    private void StatSelected()
    {
        moveStat.statPoints += 1;
        partyController.PartyUpdated.Invoke();
        levelUpPanel.move.Evolve();
        levelUpPanel.levelUpWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

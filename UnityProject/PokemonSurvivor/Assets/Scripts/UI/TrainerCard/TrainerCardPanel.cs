using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainerCardPanel : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI cardName_L;
    [SerializeField]
    public TextMeshProUGUI cardDescription_L;
    public TrainerCard trainerCard;
    public GameObject trainerCardWindow;

    [SerializeField]
    Button addToParty_B;
    public void SetTrainerCard(TrainerCard tc)
    {
        trainerCard = tc;
        cardName_L.text = trainerCard.cardName;
        cardDescription_L.text = trainerCard.cardDescription;
    }
    // Start is called before the first frame update
    void Start()
    {
        addToParty_B.onClick.AddListener(SelectCard);   
    }

    private void SelectCard()
    {
        Debug.Log(trainerCard.cardName + " selected");
        trainerCard.action.Invoke();
        trainerCardWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

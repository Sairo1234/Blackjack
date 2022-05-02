using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bets : MonoBehaviour
{
    public int Bank;
    public int Bet;

    public Text TextBank;
    public Text TextBet;

    public Button Plus10Button;
    public Button Minus10Button;
    public Button Plus100Button;
    public Button Minus100Button;
    public Button Plus1000Button;
    public Button Minus1000Button;
    public Button PlusAllButton;
    public Button MinusAllButton;
    public Button BetButton;
    // Start is called before the first frame update
    void Start()
    {
        Bank = 1000;
        Bet = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TextBank.text = Bank.ToString();
        TextBet.text = Bet.ToString();
    }

    public void Plus10()
    {
        if (Bank >= 10)
        {
            Bet += 10;
            Bank -= 10;
        }
    }
    public void Minus10()
    {
        if (Bet >= 10)
        {
            Bank += 10;
            Bet -= 10;
        }
    }
    public void Plus100()
    {
        if (Bank >= 100)
        {
            Bet += 100;
            Bank -= 100;
        }
    }
    public void Minus100()
    {
        if (Bet >= 100)
        {
            Bank += 100;
            Bet -= 100;
        }
    }
    public void Plus1000()
    {
        if (Bank >= 1000)
        {
            Bet += 1000;
            Bank -= 1000;
        }
    }
    public void Minus1000()
    {
        if (Bet >= 1000)
        {
            Bank += 1000;
            Bet -= 1000;
        }
    }
    public void PlusAll()
    {

        Bet += Bank;
        Bank = 0;

    }
    public void MinusAll()
    {
        Bank += Bet;
        Bet = 0;
    }

    public void BetButtonFunction()
    {
        this.gameObject.GetComponent<Deck>().ActivateButtons();
        this.gameObject.GetComponent<Deck>().ShuffleCards();
        this.gameObject.GetComponent<Deck>().StartGame();
        ButtonsFalse();
    }

    public void ButtonsFalse()
    {
        Plus10Button.interactable = false;
        Minus10Button.interactable = false;
        Plus100Button.interactable = false;
        Minus100Button.interactable = false;
        Plus1000Button.interactable = false;
        Minus1000Button.interactable = false;
        PlusAllButton.interactable = false;
        MinusAllButton.interactable = false;
    }

    public void ButtonsTrue()
    {
        Plus10Button.interactable = true;
        Minus10Button.interactable = true;
        Plus100Button.interactable = true;
        Minus100Button.interactable = true;
        Plus1000Button.interactable = true;
        Minus1000Button.interactable = true;
        PlusAllButton.interactable = true;
        MinusAllButton.interactable = true;
    }

    public void winBet()
    {
        Bank += 2 * Bet;
        Bet = 0;
    }

    public void looseBet()
    {
        Bet = 0;
    }

    public void tieBet()
    {
        Bank += Bet;
        Bet = 0;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;

    public GameObject dealer;
    public GameObject player;

    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;

    public Text finalMessage;
    public Text probMessage;

    public Text PlayerPoints;
    public Text DealerPoints;

    public int[] values = new int[52];
    int cardIndex = 0;

    public GameObject defaultCard;
    public string[] names = new string[52];

    public List<GameObject> InitialDeck = new List<GameObject>();
    public List<GameObject> RandomDeck = new List<GameObject>();
    public List<GameObject> ProbDeck = new List<GameObject>();

    private void Awake()
    {
        InitCardValues();

    }

    private void Start()
    {
        DeactivateButtons();
    }

    private void Update()
    {
        PlayerPoints.text = player.GetComponent<CardHand>().points.ToString();
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */

        //Cada palo tiene 13 cartas
        int[] valuesSuit = new int[13];
        int valuesSuitIndex = 0;

        //Cada card tiene el valor de su número y las especiales valen 10
        for (int i = 1; i < valuesSuit.Length; i++)
        {
            if (i < 10)
            {
                valuesSuit[i] = i + 1;
            }
            else
            {
                valuesSuit[i] = 10;
            }
        }

        //Los As valen 11 por defecto
        valuesSuit[0] = 11;

        //Ponerle valor a los elementos de la lista values
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = valuesSuit[valuesSuitIndex];
            valuesSuitIndex++;
            if (valuesSuitIndex == 13)
            {
                valuesSuitIndex = 0;
            }
        }

        //Creación de la baraja inicial ordenada
        for (int i = 0; i <= faces.Length - 1; i++)
        {
            GameObject card = Instantiate(defaultCard);
            card.name = names[i];
            card.GetComponent<CardModel>().value = values[i];
            card.GetComponent<CardModel>().front = faces[i];

            InitialDeck.Add(card);
        }
    }

    public void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */

        //Borro la baraja para crear una nueva
        RandomDeck.Clear();

        //Añado todas las cartas de la primera baraja a la nueva
        foreach (GameObject card in InitialDeck)
        {
            RandomDeck.Add(card);
        }

        //Las desordeno todas
        for (int i = 51; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            GameObject swap = RandomDeck[i];
            RandomDeck[i] = RandomDeck[j];
            RandomDeck[j] = swap;
        }

        //Hago una baraja que se usará para calcular las probabilidades
        foreach (GameObject card in RandomDeck)
        {
            ProbDeck.Add(card);
        }
        ProbDeck.Add(RandomDeck[1]);

    }

    public void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
            if (dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points == 21)
            {
                Tie();
            }
            else if (player.GetComponent<CardHand>().points == 21)
            {
                Win();
            }
            else if (dealer.GetComponent<CardHand>().points == 21)
            {
                Loose();
            }
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta       
         */


        //Primera probabilidad a calcular
        double numberP1 = 0;
        double Probability1 = 0;

        foreach (GameObject card in ProbDeck)
        {
            if (card.GetComponent<CardModel>().value
                + RandomDeck[3].gameObject.GetComponent<CardModel>().value
                > player.gameObject.GetComponent<CardHand>().points)
            {
                numberP1++;
            }

        }

        Debug.Log(RandomDeck[3].gameObject.GetComponent<CardModel>().value);

        Probability1 = (numberP1 / ProbDeck.Count) * 100;
        probMessage.text = "Probabilidad de que el dealer tenga más puntuación que el jugador: "
            + string.Format("{0:0.00}", Probability1) + "% \n";


        //Segunda probabilidad a calcular
        double numberP2 = 0;
        double Probability2 = 0;

        foreach (GameObject card in ProbDeck)
        {
            if (card.GetComponent<CardModel>().value + player.gameObject.GetComponent<CardHand>().points <= 21
                && card.GetComponent<CardModel>().value + player.gameObject.GetComponent<CardHand>().points >= 17)
            {
                numberP2++;
            }
        }

        Probability2 = (numberP2 / ProbDeck.Count) * 100;
        probMessage.text += "Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta: "
            + string.Format("{0:0.00}", Probability2) + "% \n";


        //Tercera probabilidad a calcular
        double numberP3 = 0;
        double Probability3 = 0;

        foreach (GameObject card in ProbDeck)
        {
            if (card.GetComponent<CardModel>().value + player.gameObject.GetComponent<CardHand>().points > 21)
            {
                numberP3++;
            }
        }

        Probability3 = (numberP3 / ProbDeck.Count) * 100;
        probMessage.text += "Probabilidad de que el jugador obtenga más de 21 si pide una carta: "
            + string.Format("{0:0.00}", Probability3) + "%";

    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(RandomDeck[cardIndex].GetComponent<CardModel>().front,
            RandomDeck[cardIndex].GetComponent<CardModel>().value);
        cardIndex++;

        ProbDeck.Remove(RandomDeck[cardIndex]);
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(RandomDeck[cardIndex].GetComponent<CardModel>().front,
           RandomDeck[cardIndex].GetComponent<CardModel>().value);
        cardIndex++;

        ProbDeck.Remove(RandomDeck[cardIndex]);
        CalculateProbabilities();
    }

    public void Hit()
    {
        //Repartimos card al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        if (player.GetComponent<CardHand>().points > 21)
        {
            Loose();
        }
        else if (player.GetComponent<CardHand>().points == 21)
        {
            Win();
        }

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera card del dealer.
         */
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        while (dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        }
        if (dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
        {
            Tie();
        }
        else if (dealer.GetComponent<CardHand>().points > 21)
        {
            Win();
        }
        else if (player.GetComponent<CardHand>().points > dealer.GetComponent<CardHand>().points)
        {
            Win();
        }
        else if (player.GetComponent<CardHand>().points < dealer.GetComponent<CardHand>().points)
        {
            Loose();
        }

    }

    public void PlayAgain()
    {
        DealerPoints.text = "?";
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        ProbDeck.Clear();
        cardIndex = 0;

        DeactivateButtons();
        this.gameObject.GetComponent<Bets>().ButtonsTrue();


    }

    public void Win()
    {
        DeactivateButtons();

        finalMessage.color = Color.green;
        finalMessage.text = "Win";

        DealerPoints.text = dealer.GetComponent<CardHand>().points.ToString();
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        this.gameObject.GetComponent<Bets>().winBet();
    }
    public void Loose()
    {
        DeactivateButtons();

        finalMessage.color = Color.red;
        finalMessage.text = "Loose";

        DealerPoints.text = dealer.GetComponent<CardHand>().points.ToString();
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        this.gameObject.GetComponent<Bets>().looseBet();
    }
    public void Tie()
    {
        DeactivateButtons();

        finalMessage.color = Color.yellow;
        finalMessage.text = "Tie";

        DealerPoints.text = dealer.GetComponent<CardHand>().points.ToString();
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        this.gameObject.GetComponent<Bets>().tieBet();

    }

    public void ActivateButtons()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
    }
    public void DeactivateButtons()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;
    }
}

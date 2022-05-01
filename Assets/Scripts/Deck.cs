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

    public int[] values = new int[52];
    int cardIndex = 0;

    public GameObject defaultCard;
    public string[] names = new string[52];

    public List<GameObject> InitialDeck = new List<GameObject>();
    public List<GameObject> RandomDeck = new List<GameObject>();

    private void Awake()
    {
        InitCardValues();

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();

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

    private void ShuffleCards()
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

    }

    void StartGame()
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
            else if(dealer.GetComponent<CardHand>().points == 21)
            {
                Loose();
            }
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la card oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una card
         * - Probabilidad de que el jugador obtenga más de 21 si pide una card          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        cardIndex++;
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera card del dealer.
         */

        //Repartimos card al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera card del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */

    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    public void Win()
    {
        finalMessage.color = Color.green;
        finalMessage.text = "Win";
    }
    public void Loose()
    {
        finalMessage.color = Color.red;
        finalMessage.text = "Loose";
    }
    public void Tie()
    {
        finalMessage.color = Color.yellow;
        finalMessage.text = "Tie";

    }
}

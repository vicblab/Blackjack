﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public List<int> randomDeal;
    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        randomDeal = new List<int>();
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
        int value = 0;
        foreach (int card in values)
        {
            value++;
            values[cardIndex] = value;
            if (value == 13)
            {
                value = 0;
            }
            if (values[cardIndex] <= 13 && values[cardIndex] >= 10)
            {
                values[cardIndex] = 10;
            }
            if (values[cardIndex] == 1)
            {
                values[cardIndex] = 11;
            }
           // Debug.Log(values[cardIndex]);
            cardIndex++;
           
        }
        cardIndex = 0;
       
    
}

    private List<E> ShuffleList<E>(List<E> inputList)
    {
        List<E> randomList = new List<E>();

        System.Random r = new System.Random();
     
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
           
            randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        List<int> aux = new List<int>();
        for(int i=0; i<52; i++)
        {
            aux.Add(i);
        }
        randomDeal = ShuffleList(aux);
       /* for (int i = 0; i < 52; i++)
        {
            Debug.Log(randomDeal);
        }*/
       
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
            if (dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points != 21)
            {
                finalMessage.text = "HAHAHAHAHAHA PRINGAO, TENIAS UN SOLO TRABAJO Y LA BANCA SACÓ BLACKJACK";
            }else if (player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points != 21)
            {
                finalMessage.text = "MU BIEN SUERTUDO DE MIERDA, BLACKJACK!";
            }else if(player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points == 21)
            {
                finalMessage.text = "PERO QUE ME ESTAS CONTANDO, ESTO ES UN EMPATE";
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
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[randomDeal[cardIndex]],values[randomDeal[cardIndex]]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[randomDeal[cardIndex]], values[randomDeal[cardIndex]]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
       //  if (player.GetComponent<CardHand>().)
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "MU BIEN SUERTUDO DE MIERDA, BLACKJACK!";
        }
        if ( player.GetComponent<CardHand>().points >21)
        {
            finalMessage.text = "HAS PERDIDO HIJO DE PUTA";
        }
    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        if (dealer.GetComponent<CardHand>().points <= 16)
        {
            while (dealer.GetComponent<CardHand>().points < 17)
            {
                PushDealer();
            }
            dealer.GetComponent<CardHand>().InitialToggle();
            if ( dealer.GetComponent<CardHand>().points == 21)
            {
                finalMessage.text = "MU BIEN SUERTUDO DE MIERDA, BLACKJACK!";
            }
            if (dealer.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "AQUÍ PONE QUE HAS GANADO PERO NO SE YO E";
            }
            if(dealer.GetComponent<CardHand>().points> player.GetComponent<CardHand>().points)
            {
                finalMessage.text = "GANÓ LA BANCA WEY";
            }
            else
            {
                finalMessage.text = "AQUÍ PONE QUE HAS GANADO PERO NO SE YO E";
            }
        }
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
    
}

using System.Collections.Generic;
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
            PushDealer();
            PushPlayer();
            
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
            if (dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points != 21)
            {
                dealer.GetComponent<CardHand>().InitialToggle();
                finalMessage.text = "TENIAS UN SOLO TRABAJO Y LA BANCA SACÓ BLACKJACK";
                probMessage.text = "Prob. perder por carta oculta: - \n" +
           "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
           "Prob. pasarse: -";
            }
            else if (player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points != 21)
            {
                dealer.GetComponent<CardHand>().InitialToggle();
                finalMessage.text = "MU BIEN SUERTUDO, BLACKJACK!";
                probMessage.text = "Prob. perder por carta oculta: - \n" +
           "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
           "Prob. pasarse: -";
            }
            else if(player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points == 21)
            {
                dealer.GetComponent<CardHand>().InitialToggle();
                finalMessage.text = "PERO QUE ME ESTAS CONTANDO, ESTO ES UN EMPATE";
                probMessage.text = "Prob. perder por carta oculta: - \n" +
           "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
           "Prob. pasarse: -";
            }
        }
        
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * 1- Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * 2- Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * 3- Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
         //1------------------
        int puntuacionVisible = dealer.GetComponent<CardHand>().points - dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().value;
        int diferenciaPuntosInicial = player.GetComponent<CardHand>().points - puntuacionVisible;
        int cartasBaraja = 52 - dealer.GetComponent<CardHand>().cards.Count - player.GetComponent<CardHand>().cards.Count;
       float probDealerMasQueTu = 0;
        int valorMaximoCarta = 21 - puntuacionVisible;
        int numCartasGanadoras = 0;
        if (valorMaximoCarta > 11)
        {
            valorMaximoCarta = 11;
        }
        int cont = 0;
        
            foreach(GameObject c in  dealer.GetComponent<CardHand>().cards)
            {
            if (c != dealer.GetComponent<CardHand>().cards[0])
            {
                if (c.GetComponent<CardModel>().value > diferenciaPuntosInicial && c.GetComponent<CardModel>().value < valorMaximoCarta)
                {
                    cont++;
                }
            }
        }
        foreach (GameObject c in player.GetComponent<CardHand>().cards)
        {
           
                if (c.GetComponent<CardModel>().value > diferenciaPuntosInicial && c.GetComponent<CardModel>().value < valorMaximoCarta)
                {
                    cont++;
                }
            
        }
        Debug.Log(cont);
        if (diferenciaPuntosInicial < 0)
        {
            diferenciaPuntosInicial = 0;
        }
        if (valorMaximoCarta < 10 && diferenciaPuntosInicial>0)
        {
            numCartasGanadoras = (valorMaximoCarta - diferenciaPuntosInicial) * 4 - cont;
        } else if(valorMaximoCarta == 10)
        {
            numCartasGanadoras = (10 - diferenciaPuntosInicial) * 4 + 8 - cont;
        }
        else
        {
            numCartasGanadoras = (11 - diferenciaPuntosInicial) * 4 + 8 - cont;
        }

        probDealerMasQueTu = ((float)numCartasGanadoras / (float)cartasBaraja) * 100;
        if (probDealerMasQueTu < 0)
        {
            probDealerMasQueTu = 0;
        }

        //2 ----------------------

        int diecisiete = 17 - player.GetComponent<CardHand>().points;
        int blackjack =  21 - player.GetComponent<CardHand>().points;
        int cont2 = 0;
        float probPuntuacionAlta = 0;
        if (diecisiete < 0)
        {
            diecisiete = 0;
        }
        if (diecisiete > 11)
        {
            diecisiete = 11;
        }
        if (blackjack < 0)
        {
            blackjack = 0;
        }
        if (blackjack > 11)
        {
            blackjack = 11;
        }
        foreach (GameObject c in dealer.GetComponent<CardHand>().cards)
        {
           
                if (c.GetComponent<CardModel>().value >= diecisiete && c.GetComponent<CardModel>().value <= blackjack)
                {
                    cont2++;
                }
            
        }
        bool isAce = false;
        foreach (GameObject c in player.GetComponent<CardHand>().cards)
        {

            if (c.GetComponent<CardModel>().value >= diecisiete && c.GetComponent<CardModel>().value <= blackjack)
            {
                cont2++;
            }
            if (c.GetComponent<CardModel>().value == 11)
            {
                isAce = true;
            }

        }
        if (blackjack < 10)
        {
            numCartasGanadoras = (blackjack - diecisiete) * 4 - cont2;
        }else
        {
            numCartasGanadoras = (blackjack - diecisiete) * 4 +8- cont2;
        }
        if (numCartasGanadoras < 0)
        {
            numCartasGanadoras = 0;
        }
        probPuntuacionAlta= ((float)numCartasGanadoras / (float)cartasBaraja) * 100;
        if (probPuntuacionAlta < 0)
        {
            probPuntuacionAlta = 0;
        }

        //3 --------------------------------------------------------------------------------------

       float probMenosDiecisiete = 0;
        if (isAce && player.GetComponent<CardHand>().points<12)
        {
            diecisiete += 10;
        }
            if (diecisiete < 10)
            {
                numCartasGanadoras = (diecisiete) * 4 - cont2;
            }
            else
            {
                numCartasGanadoras = (diecisiete) * 4 + 8 - cont2;
            }
        
       
        
        if (numCartasGanadoras < 0)
        {
            numCartasGanadoras = 0;
        }
        probMenosDiecisiete = ((float)numCartasGanadoras / (float)cartasBaraja) * 100;
        if (probMenosDiecisiete < 0)
        {
            probMenosDiecisiete = 0;
        }
        float probPasarse = 100 - probPuntuacionAlta - probMenosDiecisiete;
        if (probPasarse < 0)
        {
            probPasarse = 0;
        }
        probMessage.text = "Prob.perder por carta oculta: "+probDealerMasQueTu.ToString()+"%\n " +
            "Prob. obtener entre 17 y 21 en la siguiente: " + probPuntuacionAlta+ "%\n " +
            "Prob. pasarse: "+ probPasarse+"%";
        Debug.Log(diferenciaPuntosInicial);
        Debug.Log(numCartasGanadoras);
        Debug.Log(puntuacionVisible);
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
        if (finalMessage.text == "")
        {
            PushPlayer();

            /*TODO:
             * Comprobamos si el jugador ya ha perdido y mostramos mensaje
             */
            if (player.GetComponent<CardHand>().points == 21)
            {
                dealer.GetComponent<CardHand>().InitialToggle();
                finalMessage.text = "MU BIEN SUERTUDO, BLACKJACK!";
                probMessage.text = "Prob. perder por carta oculta: - \n" +
            "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
            "Prob. pasarse: -";
            }
            if (player.GetComponent<CardHand>().points > 21)
            {
                dealer.GetComponent<CardHand>().InitialToggle();
                finalMessage.text = "HAS FRACASADO COMO EN TODO EN TU VIDA";
                probMessage.text = "Prob. perder por carta oculta: - \n" +
           "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
           "Prob. pasarse: -";
            }
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
        if (finalMessage.text == "")
        {
            while (dealer.GetComponent<CardHand>().points < 17)
            {
                PushDealer();
            }
            dealer.GetComponent<CardHand>().InitialToggle();
            if (dealer.GetComponent<CardHand>().points == 21)
            {
                finalMessage.text = "HAHAHA SACÓ BLACKJACK EL OTRO";
                probMessage.text = "Prob. perder por carta oculta: - \n" +
           "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
           "Prob. pasarse: -";
            }
            else if (dealer.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "AQUÍ PONE QUE HAS GANADO PERO NO SE YO E";
                probMessage.text = "Prob. perder por carta oculta: - \n" +
           "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
           "Prob. pasarse: -";
            }
            else if (dealer.GetComponent<CardHand>().points < 21)
            {
                if (dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points)
                {
                    finalMessage.text = "GANÓ LA BANCA WEY";
                    probMessage.text = "Prob. perder por carta oculta: - \n" +
            "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
            "Prob. pasarse: -";
                }
                else if(dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points)
                {
                    finalMessage.text = "AQUÍ PONE QUE HAS GANADO PERO NO SE YO E";
                    probMessage.text = "Prob. perder por carta oculta: - \n" +
            "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
            "Prob. pasarse: -";
                }
                else if(dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
                {
                    finalMessage.text = "PERO QUE ME ESTAS CONTANDO, ESTO ES UN EMPATE";
                    probMessage.text = "Prob. perder por carta oculta: - \n" +
            "Prob. obtener entre 17 y 21 en la siguiente: - \n " +
            "Prob. pasarse: -";
                }
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

using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{

    public List<Card> cards = new List<Card>();

    private System.Random random = new System.Random();
    private List<Card> availableCards = new List<Card>();

    public void addCard(Card card){
        card.Index = cards.Count;
        this.cards.Add(card);
    }   


    public CardInHand drawCard()
    {
        availableCards.Clear();

        for (int i = 0; i < cards.Count; i++)
        {
            for (int y = 0; y < cards[i].availableCards(); y++)
            {
                availableCards.Add(cards[i]);
            }
        }

        if (availableCards.Count <= 0) return new CardInHand(cards[0]);
        return new CardInHand(availableCards[random.Next(availableCards.Count)].getCard());
    }

    public Card getCard(int index)
    {
        return null;
    }
}
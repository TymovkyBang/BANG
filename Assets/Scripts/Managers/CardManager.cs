using System;
using System.Collections.Generic;

public class CardManager
{

    public List<Card> cards = new List<Card>();


    public void addCard(Card card){
        card.Index = cards.Count;
        this.cards.Add(card);
    }   

    public Card drawCard(){
        var random = new Random();
        return cards[random.Next(cards.Count)];
    }
}
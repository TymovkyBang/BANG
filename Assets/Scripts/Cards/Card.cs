
using System;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] protected int index;
    public new string name;
    public string description;
    public Sprite artwork;
    public Sprite background;

    public int numberOfCards;

    public int numberOfCardsCurrently = 0;

    void Start() {
        GameManager.cardManager.addCard(this);
        this.init();
        if (GameManager.playerList.Count > 3) this.numberOfCards *= (int)(GameManager.playerList.Count/3);
    }

    public int Index{
        get {
            return this.index;
        }
        set {
            this.index = value; 
        }
    }

    public abstract bool use();

    public int availableCards()
    {
        return this.numberOfCards - this.numberOfCardsCurrently;
    }
    
    public Card getCard()
    {
        this.numberOfCardsCurrently++;
        Debug.Log(this.name + availableCards());
        return this;
    }

    protected abstract void init();

}

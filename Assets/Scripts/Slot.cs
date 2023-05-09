using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{   
    [SerializeField] int index;
    private bool available = true;
    private CardInHand cardInHand = null;

    [SerializeField] GameObject backgroundObject;
    [SerializeField] GameObject artworkObject;
    [SerializeField] GameObject descriptionObject;
    [SerializeField] GameObject nameObject;
    [SerializeField] GameObject markObject;

    void Start() {
        GameManager.slotManager.addSlot(this);
        this.gameObject.SetActive(false);
        this.Index = this.index;
    }

    public int Index{
        get {
            return this.index;
        }
        set {
            this.index = value; 
            gameObject.name = "Slot " + this.index.ToString();
        }
    }

    public CardInHand CardInHand{
        set {
            setCard(value);
        }
        get {
            return this.cardInHand;
        }
    }

    public void empty(){
        gameObject.SetActive(false);
		this.cardInHand = null;
		this.available = true;
    }

    private void setCard(CardInHand card){
        gameObject.SetActive(true);
        this.cardInHand = card;
        this.available = false;

        this.nameObject.GetComponent<TextMeshProUGUI>().text = this.cardInHand.card.name;
        this.descriptionObject.GetComponent<TextMeshProUGUI>().text = this.cardInHand.card.description;
        this.artworkObject.GetComponent<Image>().sprite = this.cardInHand.card.artwork;
        this.backgroundObject.GetComponent<Image>().sprite = this.cardInHand.card.background;
    }

    public bool Available  {
        get {
            return this.available;
        }
    }

    public void use(){
		GameManager.localPlayer.playCard(this.index + GameManager.slotManager.shift);
    }
}

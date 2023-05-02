using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{   
    [SerializeField] int index;
    private bool available = true;
    private Card card = null;

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

    public Card Card{
        set {
            setCard(value);
        }
        get {
            return this.card;
        }
    }

    public void empty(){
        gameObject.SetActive(false);
        this.card = null;
        this.available = true;
        GameManager.slotManager.full = false;
    }

    private void setCard(Card card){
        gameObject.SetActive(true);
        this.card = card;
        this.available = false;

        gameObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = this.card.name;
        gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = this.card.description;
        gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = this.card.artwork;
        gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.card.background;
    }

    public bool Available{
        get {
            return this.available;
        }
    }

    public void use(){
        GameManager.localPlayer.playCard(this.index + GameManager.slotManager.shift);
    }
}

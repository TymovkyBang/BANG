using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class SlotManager
{

    public List<Slot> slots = new List<Slot>();
    public int shift = 0;


    public GameObject shiftLeftButton;
    public GameObject shiftRightButton;
    public void addSlot(Slot slot){
        slots.Add(slot);
    }   

    public Slot getSlot(int index){
        for (int i = 0; i < slots.Count; i++) if (slots[i].Index == index) return slots[i];
        return null;
    }

    public void visibility(bool visible)
    {
		foreach (Slot slot in slots) { slot.gameObject.SetActive(visible); }
		shiftLeftButton.SetActive(visible);
        shiftRightButton.SetActive(visible);
	}

    public void update(){
        
        int cycles = GameManager.localPlayer.CardsInHand.Count;
        if (this.shift + this.slots.Count > GameManager.localPlayer.CardsInHand.Count) {
            this.shift = Math.Max(GameManager.localPlayer.CardsInHand.Count - this.slots.Count, 0);
        }
        if (this.shift + this.slots.Count < GameManager.localPlayer.CardsInHand.Count) {
            this.shiftRightButton.SetActive(true);
            this.shiftRightButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = (GameManager.localPlayer.CardsInHand.Count - this.slots.Count - this.shift).ToString();
        }
        else {
            this.shiftRightButton.SetActive(false);
        }
        if (this.shift > 0) {
            this.shiftLeftButton.SetActive(true);
            this.shiftLeftButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = this.shift.ToString();
        }
        else {
            this.shiftLeftButton.SetActive(false);
        }

        for (int i = 0; i < this.slots.Count; i++){
            if (i < cycles) this.getSlot(i).CardInHand = GameManager.localPlayer.CardsInHand[i + this.shift];
            else this.getSlot(i).empty();
            
        }
    }

    public void shiftRight(){
        this.shift++;
        this.update();
    }

    public void shiftLeft(){
        if (this.shift > 0) {
            this.shift--;
            this.update();
        }
    }
}

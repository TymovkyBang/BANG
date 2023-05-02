using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int maxHealth, health;
    private Role role;
    private Character character;
    private List<Card> hand = new List<Card>();

    public Player(Character character, Role role) {
        this.character = character;
        this.role = role;
        this.maxHealth = character.Health + role.BonusHealth;
        this.health = this.maxHealth;
    }

    public int Health {
        get {
            return this.health;
        }
        set {
            if (this.health + value <= 0) {
                this.health = 0;
                this.kill();
            }
            else if (this.health + value >= this.maxHealth) {
                this.health = this.maxHealth;
            }
            else {
                this.health += value;
            }
            GameManager.healthManager.update();
        }
    }

    private void kill() {
        Debug.Log(this.character.Name[0] + " has died!\nHis role was " + this.role.Name + "!");
    }

    public void drawCard(){
        this.hand.Add(GameManager.cardManager.drawCard());

        if (GameManager.slotManager.slots.Count - this.hand.Count < 0) GameManager.slotManager.shift++;
        if (GameManager.slotManager.slots.Count + GameManager.slotManager.shift < this.hand.Count) GameManager.slotManager.shift = this.hand.Count - GameManager.slotManager.slots.Count;

        GameManager.slotManager.update();
    }

    public void playCard(int i){
        this.hand[i].use();
        this.hand.RemoveAt(i);
        GameManager.slotManager.update();
    }

    public List<Card> CardsInHand {
        get {
            return this.hand;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
	private int maxHealth, health, index;
	private Role role;
	private Character character;
	private List<CardInHand> hand = new List<CardInHand>();
	public ulong id;
	public bool usedBang = false;
	private int cardsToDraw = 0;
	public bool dead = false;

	public bool onCooldown = false;

	public bool beingShotAt = false;

	public bool hasBarrel = false;
	public bool hasDynamite = false;
	public bool hasJail = false;
	public bool hasVolcanic = false;

	public Player(ulong id)
	{
		this.character = GameManager.joe;
		
		this.index = GameManager.playerList.Count;
		this.id = id;
		
		GameManager.localPlayer = this; 

	}


	public int CardsToDraw
	{
		get { return cardsToDraw; }
		set 
		{ 
			cardsToDraw = value;
			GameManager.drawButton.SetActive(cardsToDraw > 0);
			GameManager.drawButton.GetComponentInChildren<TextMeshProUGUI>().text = (CardsToDraw.ToString());
		}
	}

	public int MaxHealth
	{
		get
		{
			return maxHealth;
		}
	}
	public int Health
	{
		get
		{
			return this.health;
		}
		set
		{
			if (this.health + value <= 0)
			{
				this.health = 0;
				this.kill();
			}
			else if (this.health + value >= this.maxHealth)
			{
				this.health = this.maxHealth;
			}
			else
			{
				this.health += value;
			}
			GameManager.healthManager.update();
		}
	}

	public Character Character { get { return this.character; } }

	private void kill()
	{
		GameManager.slotManager.visibility(false);
		GameManager.deathScreen.SetActive(true);
		dead = true;
		GameManager.localNetwork.dieServerRpc(GameManager.localNetwork.ID, GameManager.localNetwork.roleID);
	}

	public void drawCard()
	{
		if (CardsToDraw > 0)
		{
			CardsToDraw--;
			this.hand.Add(GameManager.cardManager.drawCard());

			if (GameManager.slotManager.slots.Count - this.hand.Count < 0) GameManager.slotManager.shift++;
			if (GameManager.slotManager.slots.Count + GameManager.slotManager.shift < this.hand.Count) GameManager.slotManager.shift = this.hand.Count - GameManager.slotManager.slots.Count;
				
			GameManager.slotManager.update();
		}
	}

	public void playCard(int i)
	{
		if (GameManager.localPlayer.beingShotAt && hand[i].card is Missed)
		{
			if (!hand[i].card.use()) return;
			this.hand.RemoveAt(i);
			GameManager.slotManager.update();
			return;
		}

		if (GameManager.localNetwork.indians && !GameManager.localNetwork.onTurn && hand[i].card is Bang)
			{
			GameManager.localNetwork.usedBangAgainstIndians = true;
			this.hand.RemoveAt(i);
			GameManager.slotManager.update();
			return;
			}

		if (!GameManager.localNetwork.onTurn || hand[i].card is Targetable && GameManager.selectedPlayer == null)
		{
			return;
		}

		if (!hand[i].card.use()) return;
		this.hand.RemoveAt(i);
		GameManager.slotManager.update();
	}

	public void trashCard(int i)
	{
		this.hand[i].card.numberOfCardsCurrently--;
		this.hand.RemoveAt(i);
		GameManager.slotManager.update();
	}

	public List<CardInHand> CardsInHand
	{
		get
		{
			return this.hand;
		}
	}

	public void startRound()
	{
		this.usedBang = false;

		if (hasDynamite && GameManager.chance())
		{
			GameManager.console.log("Too bad! Your dynamite exploded! You lost 3 lives.", "red");
			Health = -3;
			hasDynamite = false;
		} else if (hasDynamite)
		{
			GameManager.localNetwork.sendDynamiteServerRpc((int)GameManager.localNetwork.ID);
			hasDynamite = false;
		}

		if (hasJail && !GameManager.chance()) {
			GameManager.console.log("Too bad! You'll rot in Jail for another round.", "yellow");
			GameManager.localNetwork.nextRound();
			return;
		} else if (hasJail)
		{
			GameManager.console.log("You're lucky! You've been freed from the Jail.", "green");
			hasJail = false;
		}

		CardsToDraw += 2;
	}
	public bool endRound()
	{
		if (this.hand.Count > this.health || this.cardsToDraw > 0 || this.onCooldown) return false;
		GameManager.localNetwork.nextRound();
		return true;
	}

	public async void getShot()
	{
		if (hasBarrel && GameManager.chance())
		{
			GameManager.console.log("You're lucky! Your barrel has saved you.", "green");
			return;
		}

		beingShotAt = true;

		await Task.Delay(GameManager.cooldownTime * 1000);


		if (beingShotAt)
		{
			Health = -1;
			GameManager.console.log("You've been shot! Losing 1 life.", "red");
			beingShotAt = false;
			return;
		}


		GameManager.console.log("You've successfully dodged the shot!", "green");
	}

	public Role Role { get { return this.role; } set { 
			this.role = value;
			this.maxHealth = this.character.Health + this.role.BonusHealth;
			Health = this.maxHealth;
			CardsToDraw += Health;
		} }
}

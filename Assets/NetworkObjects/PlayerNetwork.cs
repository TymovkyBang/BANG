using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
	public string nickname;
	private ulong id;
	private bool host;
	public bool onTurn = false;
	public int roleID, characterID;
	public NetworkVariable<int> numberOfPlayers = new NetworkVariable<int>(0);
	public int sheriffID;
	public bool indians = false;
	public bool usedBangAgainstIndians = false;

	public int outlaws = 0;
	public int deputies = 0;
	public int renegades = 0;

	private int whoseTurnItIs = 0;

	public GameObject hasBarrelObject;
	public GameObject hasVolcanicObject;
	public GameObject hasDynamiteObject;
	public GameObject hisTurnObject;

	public string globalMessageColor = "white";
	public string attackerColor = "green";
	public string cardNameColor = "yellow";
	public string normalColor = "white";
	public string warningColor = "red";

	private string roleListString = "";
	public int WhoseTurnItIs
	{
		get { return whoseTurnItIs; }
		set { whoseTurnItIs = value; }
	}
	public ulong ID
	{
		get { return id; }
	}

	public bool isHost
	{
		get { return this.host; }
	}
	public void Start()
	{
		this.id = OwnerClientId;
		this.nickname = "Player " + this.id;
		this.host = IsHost;

		playerJoinedServerRpc();

		GameManager.allNetworks.Add(this);

		if (!IsOwner) {
			GameManager.otherNetworks.Add(this);
			return;
		}

		gameObject.SetActive(false);
		GameManager.localNetwork = this;
		new Player(id);
		this.characterID = GameManager.localPlayer.Character.Index;

		Debug.Log(GameManager.localNetwork.ID);

		// SERVER ONLY
		if (!IsHost) return;

	}

	// Pro každý ClientRPC musí být vlastní ServerRPC, protože ClientRPC musí být spuštěno ze serveru, because it's acting funny otherwise

	[ServerRpc(RequireOwnership = false)]
	public void playerJoinedServerRpc()
	{
		numberOfPlayers.Value++;
	}

	[ServerRpc(RequireOwnership = false)]
	public void setTurnServerRpc(int who)
	{
		Debug.Log("SET TURN SERVER RPC");
		setTurnClientRpc(who);
	}
	[ClientRpc]
	public void setTurnClientRpc(int who)
	{
		GameManager.localNetwork.WhoseTurnItIs = who;
		if ((ulong)who != GameManager.localNetwork.ID) return;
		if (GameManager.localPlayer.dead) setTurnServerRpc((++who) % numberOfPlayers.Value);
		else GameManager.localPlayer.startRound();
	}

	[ServerRpc(RequireOwnership = false)]
	public void startIndiansServerRpc(ulong who)
	{
		startIndiansClientRpc(who);
	}
	[ClientRpc]
	public void startIndiansClientRpc(ulong who)
	{
		GameManager.console.log($"Indians! You have {GameManager.cooldownTime} seconds to use BANG!", "red");
		GameManager.localNetwork.indians = true;
		if (GameManager.localNetwork.ID == who)
		{
			GameManager.localNetwork.usedBangAgainstIndians = true;
			return;
		}
	}

	[ServerRpc(RequireOwnership = false)]
	public void endIndiansServerRpc()
	{
		endIndiansClientRpc();
	}
	[ClientRpc]
	public void endIndiansClientRpc()
	{
		GameManager.console.log("Indians over!");
		GameManager.localNetwork.indians = false;
		if (!GameManager.localNetwork.usedBangAgainstIndians) GameManager.localPlayer.Health = -1;
		GameManager.localNetwork.usedBangAgainstIndians = false;
	}

	public void nextRound()
	{
		Debug.Log("NEXT ROUND; Number of Outlaws: " + GameManager.localNetwork.outlaws);

		setTurnServerRpc((whoseTurnItIs + 1) % numberOfPlayers.Value);
	}

	[ServerRpc(RequireOwnership = false)]
	public void selectRoleServerRpc(ulong playerID, int roleIndex)
	{ 
		selectRoleClientRpc(playerID, roleIndex);
	}
	[ClientRpc]
	private void selectRoleClientRpc(ulong playerID, int roleIndex)
	{
		roleListString += $"<color=\"blue\">Player {playerID}<color=\"white\">  -  <color=\"green\">{GameManager.getRole(roleIndex).Name}\n";
		if (GameManager.getRole(roleIndex) is Outlaw) outlaws++;
		if (GameManager.getRole(roleIndex) is Renegade) renegades++;
		if (GameManager.getRole(roleIndex) is Deputy) deputies++;
		if (GameManager.localNetwork.ID != playerID) return;
		GameManager.localPlayer.Role = GameManager.getRole(roleIndex);
		GameManager.localNetwork.roleID = roleIndex;
		GameManager.setInfoHUD();
	}

	[ServerRpc(RequireOwnership = false)]
	public void setSheriffIDServerRpc(int ID)
	{
		setSheriffIDClientRpc(ID);
	}
	[ClientRpc]
	private void setSheriffIDClientRpc(int ID)
	{
		Debug.Log(ID);
		GameManager.localNetwork.sheriffID = ID;
		GameManager.sitDown(); // TEST
		Debug.Log("SET SHERIFF ID CLIENT RPC");
		if (this.host) GameManager.localNetwork.setTurnServerRpc(ID);
	}

	[ServerRpc(RequireOwnership = false)]
	public void dealDamageServerRpc(ulong attackerID, ulong targetID)
	{
		dealDamageClientRpc(attackerID, targetID);
	}
	[ClientRpc]
	public void dealDamageClientRpc(ulong attackerID,  ulong targetID) {
		GameManager.console.log($"Player {attackerID} attacked Player {targetID}.", "red");
		if (GameManager.localNetwork.ID != targetID) return;
		GameManager.localPlayer.Health = -1;
	}

	[ServerRpc(RequireOwnership = false)]
	public void shootServerRpc(ulong attackerID, int targetID, string cardName)
	{
		shootClientRpc(attackerID, targetID, cardName);
	}
	[ClientRpc]
	public void shootClientRpc(ulong attackerID, int targetID, string cardName)
	{
		if (targetID != -1) // -1 => Globální
		{
			if (GameManager.localNetwork.ID != (ulong)targetID)
			{
				GameManager.console.log($"Player <color=\"{attackerColor}\">{attackerID}<color=\"{normalColor}\"> used <color=\"{cardNameColor}\">{cardName}<color=\"{normalColor}\"> on Player <color=\"{attackerColor}\">{targetID}<color=\"{normalColor}\">.", normalColor);
				return;
			}

			GameManager.console.log($"Player <color=\"{attackerColor}\">{attackerID}<color=\"{warningColor}\"> used <color=\"{cardNameColor}\">{cardName}<color=\"{warningColor}\"> on <color=\"{normalColor}\">You<color=\"{warningColor}\">! You have 10 seconds to use '<color=\"{attackerColor}\">Missed<color=\"red\">'!", warningColor);
			GameManager.localPlayer.getShot();
		}
		else
		{
			if (GameManager.localNetwork.ID != attackerID)
			{
				GameManager.console.log($"Player <color=\"{attackerColor}\">{attackerID}<color=\"{warningColor}\"> used <color=\"{cardNameColor}\">{cardName}<color=\"{warningColor}\">! You have 10 seconds to use '<color=\"{attackerColor}\">Missed<color=\"{warningColor}\">'!", warningColor);
				GameManager.localPlayer.getShot();
			}
		}
	}

	[ServerRpc(RequireOwnership = false)]
	public void jailServerRpc(ulong attackerID, ulong targetID)
	{
		jailClientRpc(attackerID, targetID);
	}
	[ClientRpc]
	public void jailClientRpc(ulong attackerID, ulong targetID)
	{
		if (targetID != GameManager.localNetwork.ID) return;

		if (GameManager.localPlayer.hasJail) return;

		GameManager.console.log($"<color=\"{attackerColor}\">Player {attackerID} <color=\"red\">put You in a Jail!", "red");
		GameManager.localPlayer.hasJail = true;
	}

	[ServerRpc(RequireOwnership = false)]
	public void globalMessageServerRpc(string message)
	{
		globalMessageClientRpc(message);
	}
	[ClientRpc]
	public void globalMessageClientRpc(string message)
	{
		GameManager.console.log(message, globalMessageColor);
	}

	// REWORK - nvm it works don't touch it

	[ServerRpc(RequireOwnership = false)]
	public void dieServerRpc(ulong victimID, int roleID)
	{
		dieClientRpc(victimID, roleID);
		deathServerRpc(roleID);
	}
	[ClientRpc]
	public void dieClientRpc(ulong victimID, int roleID)
	{
		GameManager.console.log($"Player {victimID} has died! His role was <color=\"blue\">{GameManager.getRole(roleID).Name}<color=\"red\">.", "red");
		GameManager.allNetworks[(int)victimID].gameObject.SetActive(false);
	}

	[ServerRpc(RequireOwnership = false)]
	public void deathServerRpc(int roleID)
	{


		if (GameManager.getRole(roleID) is Outlaw) GameManager.localNetwork.outlaws--;
		else if (GameManager.getRole(roleID) is Deputy) GameManager.localNetwork.deputies--;
		else if (GameManager.getRole(roleID) is Renegade) GameManager.localNetwork.renegades--;

		if (GameManager.localNetwork.outlaws == 0 && GameManager.localNetwork.renegades == 0) endGameClientRpc("Sheriff won!");
		else if (GameManager.getRole(roleID) is Sheriff && GameManager.localNetwork.renegades != 0 && GameManager.localNetwork.outlaws == 0 && GameManager.localNetwork.deputies == 0) endGameClientRpc("Outlaws won!");
		else if (GameManager.getRole(roleID) is Sheriff) endGameClientRpc("Outlaws won!");
	}

	[ServerRpc(RequireOwnership = false)]
	public void startGameServerRpc()
	{
		startGameClientRpc();
	}
	[ClientRpc]
	public void startGameClientRpc()
	{
		GameManager.startGame();
	}

	[ServerRpc(RequireOwnership = false)]
	public void sendDynamiteServerRpc(int senderID)
	{
		sendDynamiteClientRpc(senderID);
	}
	[ClientRpc]
	public void sendDynamiteClientRpc(int senderID)
	{
		senderID = (senderID + 1) % numberOfPlayers.Value;
		if (senderID != (int)GameManager.localNetwork.ID) return;
		if (GameManager.localPlayer.dead) {
			sendDynamiteServerRpc((++senderID) % numberOfPlayers.Value);
			return;
		}
		GameManager.localPlayer.hasDynamite = true;
		
	}

	[ServerRpc(RequireOwnership = false)]
	public void catBalouServerRpc(ulong attackerID, ulong targetID)
	{
		catBalouClientRpc(attackerID, targetID);
	}
	[ClientRpc]
	public void catBalouClientRpc(ulong attackerID, ulong targetID)
	{
		if (targetID != GameManager.localNetwork.ID) {
			GameManager.console.log($"Player <color=\"{attackerColor}\">{attackerID}<color=\"{normalColor}\"> used <color=\"{cardNameColor}\">Cat Balou<color=\"{normalColor}\"> on Player <color=\"{attackerColor}\">{targetID}<color=\"{normalColor}\">.", normalColor);
			return;
		}
		GameManager.console.log($"Player <color=\"{attackerColor}\">{attackerID}<color=\"{warningColor}\"> used <color=\"{cardNameColor}\">Cat Balou<color=\"{warningColor}\"> on <color=\"{attackerColor}\">You<color=\"{warningColor}\">!", warningColor);

		System.Random rand = new System.Random();
		GameManager.localPlayer.trashCard(rand.Next(GameManager.localPlayer.CardsInHand.Count));
	}


	[ServerRpc(RequireOwnership = false)]
	public void generalStoreServerRpc(ulong userID)
	{
		globalMessageServerRpc($"Player {userID} used General Store!");
		generalStoreClientRpc();
	}

	[ClientRpc]
	public void generalStoreClientRpc()
	{
		GameManager.localPlayer.CardsToDraw++;
	}

	[ServerRpc(RequireOwnership = false)]
	public void saloonServerRpc(ulong userID)
	{
		globalMessageServerRpc($"Player {userID} used Saloon! Cheers!");
		saloonClientRpc();
	}

	[ClientRpc]
	public void saloonClientRpc()
	{
		GameManager.localPlayer.Health++;
	}

	[ServerRpc(RequireOwnership = false)]
	public void endGameServerRpc(string titles)
	{
		endGameClientRpc(titles);
	}

	[ClientRpc]
	public void endGameClientRpc(string title)
	{
		GameManager.endScreen.SetActive(true);
		GameManager.endScreen.GetComponentInChildren<TextMeshProUGUI>().text = $"<color=\"white\">{title}\n{roleListString}";
	}

	[ServerRpc(RequireOwnership = false)]
	public void syncIconsServerRpc(ulong playerToSyncID, bool hasBarrel, bool hasVolcanic, bool hasDynamite, bool hisTurn)
	{
		syncIconsClientRpc(playerToSyncID, hasBarrel, hasVolcanic, hasDynamite, hisTurn);
	}

	[ClientRpc]
	public void syncIconsClientRpc(ulong playerToSyncID, bool hasBarrel, bool hasVolcanic, bool hasDynamite, bool hisTurn)
	{
		foreach (PlayerNetwork network in GameManager.allNetworks)
		{
			if (network.ID == playerToSyncID)
			{
				GameManager.updateCardsOnTableHUD();
				network.hisTurnObject.SetActive(hisTurn);
				network.hasBarrelObject.SetActive(hasBarrel);
				network.hasVolcanicObject.SetActive(hasVolcanic);
				network.hasDynamiteObject.SetActive(hasDynamite);
			}
		}
	}
}


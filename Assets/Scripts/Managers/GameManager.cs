using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using System.Collections;
using TMPro;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour {
    
    // Managers

    public static CardManager cardManager = new CardManager();
    public static SlotManager slotManager = new SlotManager();
    public static HealthManager healthManager = new HealthManager();
    public static LogManager console = new LogManager();

	// Roles

	public Sheriff sheriff = new Sheriff();      // Šerif
    public Deputy deputy = new Deputy();         // Pomocník Šerifa
    public Renegade renegade = new Renegade();   // Odpadlík
    public Outlaw outlaw = new Outlaw();         // Bandita

    // Characters

    public static Joe joe = new Joe(); //(Mama)

	// Player

	public static List<Player> playerList = new List<Player>();

    public static Player localPlayer = null;
    public static PlayerNetwork localNetwork = null;

    public static List<PlayerNetwork> allNetworks = new List<PlayerNetwork>();
    public static List<PlayerNetwork> otherNetworks = new List<PlayerNetwork>();

	// Objects

	[SerializeField] GameObject shiftLeftButton;
    [SerializeField] GameObject shiftRightButton;
    [SerializeField] GameObject healthBarObject;
    [SerializeField] GameObject drawButtonObject;
    [SerializeField] GameObject gameCanvasObject;
    [SerializeField] Camera mainCameraObject;
    [SerializeField] GameObject endRoundObject;
    [SerializeField] GameObject deathScreenObject;
    [SerializeField] GameObject endScreenObject;
	[SerializeField] GameObject logObject;
	[SerializeField] GameObject infoHudObject;

	public static GameObject drawButton;
    public static GameObject gameCanvas;
    public static Camera mainCamera;
    public static GameObject endRoundButton;
    public static GameObject deathScreen;
    public static GameObject logObj;
	public static GameObject endScreen;
	public static GameObject infoHUD;

	public static PlayerNetwork selectedPlayer = null;

    private void Start() {
        slotManager.shiftLeftButton = this.shiftLeftButton;
        slotManager.shiftRightButton = this.shiftRightButton;
        drawButton = this.drawButtonObject;
        gameCanvas = this.gameCanvasObject;
        mainCamera = this.mainCameraObject;
        endRoundButton = this.endRoundObject;
        endScreen = this.endScreenObject;
		deathScreen = this.deathScreenObject;
		logObj = this.logObject;
        infoHUD = this.infoHudObject;

        console.setTextField(this.logObject.GetComponent<TextMeshProUGUI>());

		endRoundButton.SetActive(false);
		gameCanvasObject.SetActive(false);
	}

    // Variables

    public static List<Role> roleList = new List<Role>(); // All possible roles in game
    public static List<Chair> chairList = new List<Chair>();
    public static List<Role> rolesList = new List<Role>(); // Role object list

    public static int cooldownTime = 10;

    private float elapsed = 0f;

	// Update

	void Update()
	{

		for (int i = 0; i < allNetworks.Count; i++)
		{
            if (localNetwork.ID == (ulong)localNetwork.WhoseTurnItIs) localNetwork.onTurn = true;
            else localNetwork.onTurn = false;
		}

		if (localNetwork != null) endRoundButton.SetActive(localNetwork.onTurn);

		elapsed += Time.deltaTime;
		if (elapsed >= 1f)
		{
			elapsed = elapsed % 1f;
			if (localNetwork != null) localNetwork.syncIconsServerRpc(localNetwork.ID, localPlayer.hasBarrel, localPlayer.hasVolcanic, localPlayer.hasDynamite, localNetwork.onTurn);
		}
	}

	// Methods

    public static bool chance()
    {   
        System.Random rand = new System.Random();
        return (rand.Next(4) == 0);
    }

    public static void loadAndShuffleRoleList()
    {
        Debug.Log("Num of players:" + localNetwork.numberOfPlayers.Value);
        if (localNetwork.numberOfPlayers.Value == 1)
		{
			localNetwork.setSheriffIDServerRpc(0);
			localNetwork.selectRoleServerRpc(0, getRole("sheriff").Index);
            return;
		}
        else if (localNetwork.numberOfPlayers.Value == 2)
        {
            localNetwork.setSheriffIDServerRpc(0);
			localNetwork.selectRoleServerRpc(0, getRole("sheriff").Index);
			localNetwork.selectRoleServerRpc(1, getRole("outlaw").Index);
			return;
		}
		roleList.Add(getRole("sheriff"));
		roleList.Add(getRole("renegade"));
		roleList.Add(getRole("outlaw"));
		roleList.Add(getRole("outlaw"));
        if (localNetwork.numberOfPlayers.Value > 4) roleList.Add(getRole("deputy"));
		if (localNetwork.numberOfPlayers.Value > 5) roleList.Add(getRole("outlaw"));
		if (localNetwork.numberOfPlayers.Value > 6) roleList.Add(getRole("deputy"));

		// Shuffle list

		int n = roleList.Count;
		var rng = new System.Random();
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			Role value = roleList[k];
			roleList[k] = roleList[n];
			roleList[n] = value;
		}

		for (int i = 0; i < localNetwork.numberOfPlayers.Value; i++)
        {
            if (roleList[i] == getRole("sheriff")) localNetwork.setSheriffIDServerRpc(i);
            localNetwork.selectRoleServerRpc((ulong)i, roleList[i].Index);
        }
	}

    private static PlayerNetwork getSheriff()
    {
        for (int i = 0; i < localNetwork.numberOfPlayers.Value; i++)
        {
            if (allNetworks[i].roleID == getRole("sheriff").Index) return allNetworks[i];
        }
        return null;
    }

    public static int countUsedSlots(){
        int usedSlots = 0;
        for (int i = 0; i < slotManager.slots.Count; i++) if (!slotManager.slots[i].Available) usedSlots++;
        return usedSlots;
    }

    public static void drawCard()
    {
        localPlayer.drawCard();
    }


    public void shiftLeft(){
        slotManager.shiftLeft();
    }

    public void shiftRight(){
        slotManager.shiftRight();
    }

    public static void startGame()
    {
        if (localNetwork.isHost)
        { 
            loadAndShuffleRoleList();
		}
		gameCanvas.SetActive(true);
    }

    public static Chair getChair(int index)
    {
		for (int i = 0; i < chairList.Count; i++)
		{
			if (chairList[i].Index == index) return chairList[i];
		}
        return null;
	}
	public static Role getRole(int index)
	{
		for (int i = 0; i < rolesList.Count; i++)
		{
			if (rolesList[i].Index == index) return rolesList[i];
		}
		return null;
	}

	public static Role getRole(string name)
	{
		for (int i = 0; i < rolesList.Count; i++)
		{
			if (rolesList[i].Name.ToLower() == name.ToLower()) return rolesList[i];
		}
		return null;
	}

    public static void nextRound()
    {
		localPlayer.endRound();
    }

	public static void sitDown()
    {
       
        foreach (PlayerNetwork network in allNetworks)
        {
            Debug.Log("netID: " + network.ID + "(sheriffID: " + localNetwork.sheriffID + ")");
            if (network.ID == (ulong)localNetwork.sheriffID)
            {
                network.gameObject.GetComponentInChildren<Target>().defaultColor = Color.yellow;
            }
            else
            {
				network.gameObject.GetComponentInChildren<Target>().defaultColor = Color.black;
			}
            network.transform.position = getChair((int)network.ID).transform.position;
			network.transform.eulerAngles = new Vector3(network.transform.eulerAngles.x, getChair((int)network.ID).transform.rotation.eulerAngles.y, network.transform.eulerAngles.z);
		}
        GameManager.mainCamera.transform.position = new Vector3(getChair((int)localNetwork.ID).transform.position.x, GameManager.mainCamera.transform.position.y, getChair((int)localNetwork.ID).transform.position.z);

        mainCamera.transform.eulerAngles = new Vector3(25, getChair((int)localNetwork.ID).transform.rotation.eulerAngles.y, 0);

		localNetwork.gameObject.SetActive(false);
	}
	public static async void cooldown()
	{
		GameManager.localPlayer.onCooldown = true;

		await Task.Delay(GameManager.cooldownTime * 1000);

		GameManager.localPlayer.onCooldown = false;
	}

    public static Role getLocalPlayerRole()
    {
        return getRole(localNetwork.roleID);

	}

    public static void setInfoHUD()
    {
        TextMeshProUGUI playerName = infoHUD.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		TextMeshProUGUI role = infoHUD.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
		TextMeshProUGUI cardsOnTable = infoHUD.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        playerName.text = "<color=\"white\">" + localNetwork.nickname;
        role.text = "<color=\""+ getLocalPlayerRole().Color + "\">" + getLocalPlayerRole().Name;
        cardsOnTable.text = "";
	}

    public static void updateCardsOnTableHUD()
    {
		TextMeshProUGUI cardsOnTable = infoHUD.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        cardsOnTable.text = "\n";
        if (localPlayer.hasBarrel) cardsOnTable.text += "Barell\n";
        if (localPlayer.hasDynamite) cardsOnTable.text += "Dynamite\n";
        if (localPlayer.hasJail) cardsOnTable.text += "Jail\n";
        if (localPlayer.hasVolcanic) cardsOnTable.text += "Volcanic\n";
	}
}   
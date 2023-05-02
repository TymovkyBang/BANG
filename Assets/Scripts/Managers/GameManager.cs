using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    
    // Managers
    public static CardManager cardManager = new CardManager();
    public static SlotManager slotManager = new SlotManager();
    public static HealthManager healthManager = new HealthManager();
    
    // Roles
    public static Sheriff sheriff = new Sheriff();      // Šerif
    public static Deputy deputy = new Deputy();         // Pomocník Šerifa
    public static Renegade renegade = new Renegade();   // Odpadlík
    public static Outlaw outlaw = new Outlaw();         // Bandita

    // Characters
    public static Joe joe = new Joe();

    // Player
    public static Player localPlayer = new Player(joe, sheriff);
    public static Player remotePlayer1 = new Player(joe, deputy);       
    public static Player remotePlayer2 = new Player(joe, outlaw);
    public static Player remotePlayer3 = new Player(joe, renegade);
    
    // Objects

    [SerializeField] GameObject shiftLeftButton;
    [SerializeField] GameObject shiftRightButton;
    [SerializeField] GameObject healthBarObject;

    private void Start() {
        slotManager.shiftLeftButton = this.shiftLeftButton;
        slotManager.shiftRightButton = this.shiftRightButton;
        GameManager.healthManager.update();

    }
    
    // Variables

    public static bool localPlayerTurn = false;

    // Methods

    public static int maxUsedSlots{
        get {
            return localPlayer.Health;
        }
    }

    public static int countUsedSlots(){
        int usedSlots = 0;
        for (int i = 0; i < slotManager.slots.Count; i++) if (!slotManager.slots[i].Available) usedSlots++;
        return usedSlots;
    }

    public void drawCard(){
        localPlayer.drawCard();
    }

    public void shiftLeft(){
        slotManager.shiftLeft();
    }

    public void shiftRight(){
        slotManager.shiftRight();
    }
}
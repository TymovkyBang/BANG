using UnityEngine;
public class Gatling : Card
{
    protected override void init()
    {
        this.numberOfCards = 1;
    }

    public override bool use(){
        if (GameManager.localPlayer.onCooldown) 
        {
            GameManager.console.log("You are on cooldown!", "blue");
            return false; 
        }

		GameManager.cooldown();
		GameManager.localNetwork.shootServerRpc(GameManager.localNetwork.ID, -1, name);
		this.numberOfCardsCurrently--;
		return true;

	}
}

using System.Collections;
using UnityEngine;
public class Indians : Card
{
    protected override void init()
    {
        this.numberOfCards = 2; // 2
    }

    public override bool use(){
        if (GameManager.localNetwork.indians) return false;
		if (GameManager.localPlayer.onCooldown)
		{
			GameManager.console.log("You are on cooldown!", "blue");
			return false;
		}
		this.numberOfCardsCurrently--;
		StartCoroutine(indiansTimer());
		GameManager.cooldown();
		return true;
	}

    public IEnumerator indiansTimer()
    {
		GameManager.localNetwork.startIndiansServerRpc(GameManager.localNetwork.ID);
		yield return new WaitForSeconds(GameManager.cooldownTime);
		GameManager.localNetwork.endIndiansServerRpc();
		
	}
}

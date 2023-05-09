using UnityEngine;
public class Dynamite : Placeable
{
    protected override void init()
    {
        this.numberOfCards = 100; // 1
    }

    public override bool use(){
		if (GameManager.chance())
		{
			GameManager.console.log("Too bad! Your dynamite exploded in your hand! You lost 3 lives.", "red");
			GameManager.localPlayer.Health = -3;
			if (GameManager.localPlayer.dead) GameManager.localNetwork.nextRound();
			numberOfCardsCurrently--;
			return true;
		}

		GameManager.localNetwork.sendDynamiteServerRpc((int)GameManager.localNetwork.ID);
		numberOfCardsCurrently--;
		return true;

	}
	public override void place()
	{
		GameManager.localPlayer.hasDynamite = true;
	}
}

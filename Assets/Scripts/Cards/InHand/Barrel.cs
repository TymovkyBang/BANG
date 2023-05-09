using UnityEngine;
public class Barrel : Placeable
{
	protected override void init()
	{
		this.numberOfCards = 2;
	}

	public override bool use()
	{
		if (GameManager.localPlayer.hasBarrel)
		{
			GameManager.console.log("You already have Barrel!", "blue");
			return false;
		}
		place();
		return true;
	}

	public override void place()
	{
		GameManager.localPlayer.hasBarrel = true;
		numberOfCardsCurrently--;
	}
}

using UnityEngine;
public class Volcanic : Placeable
{
	protected override void init()
	{
		this.numberOfCards = 2;
	}

	public override bool use()
	{
		if (GameManager.localPlayer.hasVolcanic)
		{
			GameManager.console.log("You already have Volcanic!", "blue");
			return false;
		}
		place();
		return true;

	}
	public override void place()
	{
		GameManager.localPlayer.hasVolcanic = true;
		numberOfCardsCurrently--;
	}
}
